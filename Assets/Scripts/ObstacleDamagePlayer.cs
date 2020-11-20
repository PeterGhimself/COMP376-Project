using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDamagePlayer : MonoBehaviour
{
    [SerializeField] private LayerMask m_playerLayer;
    [SerializeField] private float m_damage = 1;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (m_playerLayer == (m_playerLayer | (1 << other.gameObject.layer)))
        {
            PlayerController playerCtrl = other.gameObject.GetComponent<PlayerController>();
            if (playerCtrl)
            {
                playerCtrl.DamagePlayer(m_damage);
            }
            else
            {
                Debug.LogError("No playercontroller script on " + other.gameObject.name);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_playerLayer == (m_playerLayer | (1 << other.gameObject.layer)))
        {
            PlayerController playerCtrl = other.gameObject.GetComponent<PlayerController>();
            if (playerCtrl)
            {
                playerCtrl.DamagePlayer(m_damage);
            }
            else
            {
                Debug.LogError("No playercontroller script on " + other.gameObject.name);
            }
        }
    }
}