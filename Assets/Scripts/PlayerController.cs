using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public enum Weapon
    {
        Dagger = 0,
        Sword = 1,
        Hammer = 2
    }

    public enum EAbility
    {
        Dash = 0,
        ProjectileExplosion = 1,
        Slowmo = 2
    }

    [Serializable]
    private class PlayerAbilityDefinition
    {
        public EAbility Ability = default;
        public float Cooldown = 1;
    }

    [Header("References")]
    [SerializeField] private Image m_healthBar = default;
    [SerializeField] private GameObject m_menu = default;
    [SerializeField] private LayerMask m_enemyProjectiles = default;
    [SerializeField] private PlayerWeapon[] m_weapons = default;
    [SerializeField] private PlayerProjectile[] m_projectiles = default;
    [SerializeField] private PlayerAbilityDefinition[] m_playerAbilities = default;

    [Header("Player Attributes")]
    [SerializeField] private Weapon m_chosenWeapon = default;
    [SerializeField] private EAbility chosenAbility = default;
    [SerializeField] private PlayerProjectile m_chosenProjectile = default;
    [SerializeField] private float m_meleeDamageModifier = 0f;
    [SerializeField] private float m_rangedDamageModifier = 0f;
    [SerializeField] private float m_maxHealth = 5f;
    [SerializeField] private float m_currentHealth = 5f;
    [SerializeField] private float m_walkSpeed = 1f;
    [SerializeField] private float m_dashForce = 5f;
    [SerializeField] private float m_dashInvincibilityTime = 0.5f;
    [SerializeField] private float m_invincibilityCooldown = 0.5f;

    private Rigidbody2D m_rigidbody = default;
    private Animator m_animator = default;
    private PlayerWeapon m_weapon = default;
    private float invincibilityTime = 0;

    private float attackCooldownTime = 0;
    private float projectileCooldownTime = 0;

    //Abilities
    private float abilityCooldownTime = 0;

    //Dash
    private bool dashInvincible = false;
    private float dashInvincibleTime = 0.5f;
    private Vector2 dashDirection = default;

    private UnityEvent restartEvent = default;

    //Animation const strings
    private const string k_attackAnim = "Attack";
    private const string m_playerHitAnim = "PlayerHit";
    private const string k_playerAtkSpeed = "SwingSpeed";

    //Input const strings
    private const string k_fireButton = "Fire";
    private const string k_projectileButton = "FireRanged";
    private const string k_horizontalAxis = "Horizontal";
    private const string k_verticalAxis = "Vertical";

    //UI
    private bool menuActive = false;

    public void Initialize(Weapon choice, int projectileChoice, UnityEvent restart = null)
    {
        m_chosenWeapon = choice;
        m_weapon = m_weapons[(int) choice];
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        restartEvent = restart;

        m_chosenProjectile = m_projectiles[projectileChoice];
        m_weapon.gameObject.SetActive(true);

        m_animator.SetFloat(k_playerAtkSpeed, m_weapon.SwingSpeed);

        if (restart != null)
            restartEvent = restart;

        m_currentHealth = m_maxHealth;
    }

    #region Updates

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuActive = !menuActive;
            Time.timeScale = menuActive ? 0 : 1;
            m_menu.SetActive(menuActive);
        }

        if (menuActive)
            return;

        UpdateCharacterStates();
        UpdatePlayerUI();
        UpdateAnimations();  
    }

    private void UpdatePlayerUI()
    {
        m_healthBar.fillAmount = m_currentHealth / m_maxHealth;
    }

    private void UpdateCharacterStates()
    {
        Walk();
        Attack();
        Projectile();
        PlayerAbility();

        if (invincibilityTime > 0)
            invincibilityTime -= Time.deltaTime;

        if (attackCooldownTime > 0)
            attackCooldownTime -= Time.deltaTime;

        if (projectileCooldownTime > 0)
            projectileCooldownTime -= Time.deltaTime;

        if (abilityCooldownTime > 0)
            abilityCooldownTime -= Time.deltaTime;

        if (dashInvincibleTime > 0)
            dashInvincibleTime -= Time.deltaTime;
    }

    private void Walk()
    {
        Vector2 walkVector = new Vector2(Input.GetAxisRaw(k_horizontalAxis), Input.GetAxisRaw(k_verticalAxis));

        if (Mathf.Abs(walkVector.magnitude) > 1)
        {
            walkVector = walkVector.normalized;
        }

        dashDirection = walkVector;

        transform.Translate(walkVector * m_walkSpeed * Time.deltaTime);

        float horizontalSpeed = walkVector.x;
        float verticalSpeed = walkVector.y;

        m_animator.SetFloat("HorizontalSpeed", Mathf.Abs(horizontalSpeed));
        m_animator.SetFloat("VerticalSpeed", Mathf.Abs(verticalSpeed));

        Vector3 theScale = transform.localScale;
        if (horizontalSpeed < 0)
        {
            // Multiply the player's x local scale by -1.
            if (theScale.x > 0)
            {
                // only rotate if necessary
                theScale.x *= -1;
                transform.localScale = theScale;
            }
        }
        else if (horizontalSpeed > 0)
        {
            // Multiply the player's x local scale by -1.
            if (theScale.x < 0)
            {
                // only rotate if necessary
                theScale.x *= -1;
                transform.localScale = theScale;
            }
        }
    }

    private void Attack()
    {
        if (attackCooldownTime > 0)
        {
            return;
        }

        if (Input.GetButton(k_fireButton))
        {
            m_animator.SetTrigger(k_attackAnim);
            attackCooldownTime = m_weapon.Cooldown;
        }
    }

    private void Projectile()
    {
        if (projectileCooldownTime > 0)
        {
            return;
        }

        if (Input.GetButton(k_projectileButton))
        {
            projectileCooldownTime = m_chosenProjectile.Cooldown;
            //todo: choose projectile

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            GameObject projectile = Instantiate(m_chosenProjectile.gameObject, transform.position, Quaternion.identity);

            Vector2 direction = (mousePos - (Vector2) transform.position).normalized;
            projectile.transform.right = direction;

            PlayerProjectile playerProjectile = projectile.GetComponent<PlayerProjectile>();

            if (playerProjectile)
            {
                playerProjectile.IncreaseDamage(m_meleeDamageModifier);
            }
            else
            {
                Debug.LogError("No player projectile script on " + projectile.gameObject.name);
            }

            Rigidbody2D projRB = projectile.GetComponent<Rigidbody2D>();
            if (projRB)
            {
                projRB.AddForce(direction * 5f, ForceMode2D.Impulse);
            }
        }
    }

    private void UpdateAnimations()
    {
        m_animator.SetBool(m_playerHitAnim, invincibilityTime > 0);
    }

    private void PlayerAbility()
    {
        if (Input.GetButtonDown("Ability") && abilityCooldownTime <= 0)
        {
            PlayerAbilityDefinition ability = m_playerAbilities[(int)chosenAbility];
            abilityCooldownTime = ability.Cooldown;
            if (ability.Ability == EAbility.Dash)
                DashAbility();
            else if (ability.Ability == EAbility.ProjectileExplosion)
                ProjectileExplosionAbility();
        }

        if (dashInvincibleTime < 0)
        {
            dashInvincible = false;
        }

    }

    #endregion

    public void DashAbility()
    {
        dashInvincible = true;
        dashInvincibleTime = m_dashInvincibilityTime;
        m_rigidbody.AddForce(dashDirection * m_dashForce, ForceMode2D.Impulse);
    }

    public void ProjectileExplosionAbility()
    {
        for(int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                GameObject projectile = Instantiate(m_chosenProjectile.gameObject, transform.position, Quaternion.identity);
                Vector2 direction = ((new Vector2(i, j) + (Vector2)transform.position) - (Vector2)transform.position).normalized;
                projectile.transform.right = direction;
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                if(rb)
                {
                    rb.AddForce(new Vector2(i, j).normalized * 5f, ForceMode2D.Impulse);
                }
            }
        }
    }

    public void DamagePlayer(float damage)
    {
        if (invincibilityTime > 0 || dashInvincible)
            return;

        invincibilityTime = m_invincibilityCooldown;
        m_currentHealth -= damage;

        if (m_currentHealth <= 0)
        {
            restartEvent?.Invoke();
        }
    }

    public void ChangePlayerStat(Powerup.PowerupType type, float amount)
    {
        switch (type)
        {
            case Powerup.PowerupType.Attack:
                m_meleeDamageModifier += amount;
                m_weapon.IncreaseByDamageMod(m_meleeDamageModifier);
                m_rangedDamageModifier += amount;
                break;
            case Powerup.PowerupType.Health:
                m_maxHealth += amount;
                break;
            case Powerup.PowerupType.Speed:
                m_walkSpeed += amount;
                break;
            case Powerup.PowerupType.Heal:
                m_currentHealth += amount;
                if (m_currentHealth > m_maxHealth)
                    m_currentHealth = m_maxHealth;
                break;
            default:
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_enemyProjectiles == (m_enemyProjectiles | (1 << collision.gameObject.layer)))
        {
            ProjectileScript projectile = collision.gameObject.GetComponent<ProjectileScript>();
            if (projectile)
            {
                DamagePlayer(projectile.damage);
            }
            else
            {
                Debug.LogError("No projectile script on " + projectile.gameObject.name);
            }
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        menuActive = false;
        m_menu.SetActive(false);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
