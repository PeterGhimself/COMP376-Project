using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image m_healthBar = default;
    [SerializeField] private LayerMask m_enemyProjectiles = default;

    [Header("Player Attributes")]
    [SerializeField] private float m_maxHealth = 5f;
    [SerializeField] private float m_currentHealth = 5f;
    [SerializeField] private float m_walkSpeed = 1f;
    [SerializeField] private float m_invincibilityCooldown = 0.5f;

    private Rigidbody2D m_rigidbody = default;
    private Animator m_animator = default;
    private float invicibilityTime = 1;

    //Animation const strings
    private const string k_attackAnim = "Attack";
    private const string m_playerHitAnim = "PlayerHit";

    //Input const strings
    private const string k_fireButton = "Fire";
    private const string k_horizontalAxis = "Horizontal";
    private const string k_verticalAxis = "Vertical";

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
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

        if (invicibilityTime > 0)
            invicibilityTime -= Time.deltaTime;
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
        if (Input.GetButton(k_fireButton))
        {
            m_animator.SetTrigger(k_attackAnim);
        }
    }

    private void UpdateAnimations()
    {
        m_animator.SetBool(m_playerHitAnim, invicibilityTime > 0);
    }

#endregion

    public void DamagePlayer(float damage)
    {
        if (invicibilityTime > 0)
            return;

        invicibilityTime = m_invincibilityCooldown;
        m_currentHealth -= damage;
        print(damage);
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
