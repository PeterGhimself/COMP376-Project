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
        float horizontalSpeed = Input.GetAxisRaw("Horizontal");

        if (walkVector.magnitude > 1)
            walkVector = walkVector.normalized;

        transform.Translate(walkVector * m_walkSpeed * Time.deltaTime);
        
        m_animator.SetFloat("Speed", Mathf.Abs(horizontalSpeed));

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
        if (Input.GetButton(k_fireButton))
        {
            m_animator.SetTrigger(k_attackAnim);
        }
    }
}
