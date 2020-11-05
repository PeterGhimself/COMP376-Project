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

    [Header("Player Attributes")]
    [SerializeField] private Weapon m_chosenWeapon = default;
    [SerializeField] private float m_maxHealth = 5f;
    [SerializeField] private float m_currentHealth = 5f;
    [SerializeField] private float m_walkSpeed = 1f;
    [SerializeField] private float m_invincibilityCooldown = 0.5f;

    private Rigidbody2D m_rigidbody = default;
    private Animator m_animator = default;
    private PlayerWeapon m_weapon = default;
    private float invincibilityTime = 1;
    private float attackCooldownTime = 0;
    private UnityEvent restartEvent = default;

    //Animation const strings
    private const string k_attackAnim = "Attack";
    private const string m_playerHitAnim = "PlayerHit";

    //Input const strings
    private const string k_fireButton = "Fire";
    private const string k_horizontalAxis = "Horizontal";
    private const string k_verticalAxis = "Vertical";

    public void Initialize(Weapon choice, UnityEvent restart)
    {
        m_chosenWeapon = choice;
        m_weapons[(int)choice].gameObject.SetActive(true);
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

        if (invincibilityTime > 0)
            invincibilityTime -= Time.deltaTime;

        if (attackCooldownTime > 0)
            attackCooldownTime -= Time.deltaTime;
    }

    private void Walk()
    {
        Vector2 walkVector = new Vector2(Input.GetAxisRaw(k_horizontalAxis), Input.GetAxisRaw(k_verticalAxis));

        if (walkVector.magnitude > 1)
            walkVector = walkVector.normalized;

        transform.Translate(walkVector * m_walkSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        if (attackCooldownTime > 0)
        {
            m_weapon.gameObject.SetActive(true);
            return;
        }

        m_weapon.gameObject.SetActive(false);

        if (Input.GetButton(k_fireButton))
        {
            m_animator.SetTrigger(k_attackAnim);
            attackCooldownTime = m_weapon.Cooldown;
            //todo set swing speed
        }
    }

    private void UpdateAnimations()
    {
        m_animator.SetBool(m_playerHitAnim, invincibilityTime > 0);
    }

#endregion

    public void DamagePlayer(float damage)
    {
        if (invincibilityTime > 0)
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
