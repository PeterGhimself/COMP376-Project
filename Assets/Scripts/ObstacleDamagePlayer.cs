using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDamagePlayer : MonoBehaviour
{
    [SerializeField] private LayerMask m_playerLayer;
    [SerializeField] private float m_damage = 1;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (m_playerLayer == (m_playerLayer | (1 << collider.gameObject.layer)))
        {
            PlayerController playerCtrl = collider.gameObject.GetComponent<PlayerController>();
            if (playerCtrl)
            {
                playerCtrl.DamagePlayer(m_damage);
            }
            else
            {
                Debug.LogError("No playercontroller script on " + collider.gameObject.name);
            }
        }
    }
}
