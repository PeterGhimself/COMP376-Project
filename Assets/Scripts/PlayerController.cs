using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private float m_walkSpeed = 1f;
    [SerializeField] private Image m_healthBar = default;

    [Header("Player Attributes")]
    [SerializeField] private float m_maxHealth = 5f;
    [SerializeField] private float m_currentHealth = 5f;

    private Rigidbody2D m_rigidbody = default;
    private Animator m_animator = default;

    private const string k_attackAnim = "Attack";

    private const string k_fireButton = "Fire";
    private const string k_horizontalAxis = "Horizontal";
    private const string k_verticalAxis = "Vertical";

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        UpdateCharacterStates();
        UpdatePlayerUI();
    }

    public void DamagePlayer(float damage)
    {
        m_currentHealth -= damage;
    }

    private void UpdatePlayerUI()
    {
        m_healthBar.fillAmount = m_currentHealth / m_maxHealth;
    }

    private void UpdateCharacterStates()
    {
        Walk();
        Attack();
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
}
