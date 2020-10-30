using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_walkSpeed = 1f;

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
