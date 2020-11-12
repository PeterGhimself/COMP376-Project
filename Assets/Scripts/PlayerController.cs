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

    [Header("References")]
    [SerializeField] private Image m_healthBar = default;
    [SerializeField] private LayerMask m_enemyProjectiles = default;
    [SerializeField] private PlayerWeapon[] m_weapons = default;
    [SerializeField] private GameObject m_icicleProjectile = default;

    [Header("Player Attributes")]
    [SerializeField] private Weapon m_chosenWeapon = default;
    [SerializeField] private float m_maxHealth = 5f;
    [SerializeField] private float m_currentHealth = 5f;
    [SerializeField] private float m_walkSpeed = 1f;
    [SerializeField] private float m_dashForce = 5f;
    [SerializeField] private float m_dashCooldown = 2f;
    [SerializeField] private float m_dashInvincibilityTime = 0.5f;
    [SerializeField] private float m_invincibilityCooldown = 0.5f;

    private Rigidbody2D m_rigidbody = default;
    private Animator m_animator = default;
    private PlayerWeapon m_weapon = default;
    private float invincibilityTime = 0;

    private float attackCooldownTime = 0;
    private float projectileCooldownTime = 0;

    private float dashCooldownTime = 0;
    private bool dashInvincible = false;
    private float dashInvincibleTime = 0.5f;
    private UnityEvent restartEvent = default;

    //Animation const strings
    private const string k_attackAnim = "Attack";
    private const string m_playerHitAnim = "PlayerHit";

    //Input const strings
    private const string k_fireButton = "Fire";
    private const string k_projectileButton = "FireRanged";
    private const string k_horizontalAxis = "Horizontal";
    private const string k_verticalAxis = "Vertical";

    public void Initialize(Weapon choice, UnityEvent restart)
    {
        m_chosenWeapon = choice;
        m_weapon = m_weapons[(int)choice];
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        restartEvent = restart;

        m_currentHealth = m_maxHealth;
    }

#region Updates

    void Update()
    {
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

        if (invincibilityTime > 0)
            invincibilityTime -= Time.deltaTime;

        if (attackCooldownTime > 0)
            attackCooldownTime -= Time.deltaTime;

        if (projectileCooldownTime > 0)
            projectileCooldownTime -= Time.deltaTime;

        if (dashCooldownTime > 0)
            dashCooldownTime -= Time.deltaTime;

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

        if(dashInvincibleTime < 0)
        {
            dashInvincible = false;
        }

        if (Mathf.Abs(walkVector.magnitude) > 0)
        {
            if (Input.GetButtonDown("Dash") && dashCooldownTime <= 0)
            {
                dashInvincible = true;
                dashInvincibleTime = m_dashInvincibilityTime;
                dashCooldownTime = m_dashCooldown;
                m_rigidbody.AddForce(walkVector * m_dashForce, ForceMode2D.Impulse);
            }
        }

        transform.Translate(walkVector * m_walkSpeed * Time.deltaTime);

        float horizontalSpeed = walkVector.x;
        float verticalSpeed = walkVector.y;

        m_animator.SetFloat("HorizontalSpeed", Mathf.Abs(horizontalSpeed));
        m_animator.SetFloat("VerticalSpeed", Mathf.Abs(verticalSpeed));

        Vector3 theScale = transform.localScale;
        if (horizontalSpeed < 0) {
		    // Multiply the player's x local scale by -1.
            if (theScale.x > 0) {  // only rotate if necessary
		        theScale.x *= -1;
		        transform.localScale = theScale;
            }
        } else if (horizontalSpeed > 0) {
            // Multiply the player's x local scale by -1.
            if (theScale.x < 0) { // only rotate if necessary
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
            //todo set swing speed
        }
    }

    private void Projectile()
    {
        if(projectileCooldownTime > 0)
        {
            return;
        }

        if (Input.GetButton(k_projectileButton))
        {
            projectileCooldownTime = m_weapon.Cooldown;
            //todo: choose projectile

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            GameObject projectile = Instantiate(m_icicleProjectile, transform.position, Quaternion.identity);

            Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
            projectile.transform.right = direction;

            Rigidbody2D projRB = projectile.GetComponent<Rigidbody2D>();
            if(projRB)
            {
                projRB.AddForce(direction * 5f, ForceMode2D.Impulse);
            }
        }
    }

    private void UpdateAnimations()
    {
        m_animator.SetBool(m_playerHitAnim, invincibilityTime > 0);
    }

#endregion

    public void DamagePlayer(float damage)
    {
        if (invincibilityTime > 0 || dashInvincible)
            return;

        invincibilityTime = m_invincibilityCooldown;
        m_currentHealth -= damage;

        if(m_currentHealth <= 0)
        {
            restartEvent?.Invoke();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_enemyProjectiles == (m_enemyProjectiles | (1 << collision.gameObject.layer)))
        {
            ProjectileScript projectile = collision.gameObject.GetComponent<ProjectileScript>();
            if(projectile)
            {
                DamagePlayer(projectile.damage);
            }
            else
            {
                Debug.LogError("No projectile script on " + projectile.gameObject.name);
            }
        }
    }
}
