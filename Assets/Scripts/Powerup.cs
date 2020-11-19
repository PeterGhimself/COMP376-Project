using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public enum PowerupType
    {
        Attack,
        Health,
        Speed
    }

    [SerializeField] private PowerupType m_powerupType = default;
    [SerializeField] private float m_powerupIncrease = default;
    [SerializeField] private LayerMask m_playerLayer = default;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (m_playerLayer == (m_playerLayer | (1 << collider.gameObject.layer)))
        {
            PlayerController playerCtrl = collider.gameObject.GetComponent<PlayerController>();
            if (playerCtrl)
            {
                playerCtrl.ChangePlayerStat(m_powerupType, m_powerupIncrease);
            }
            else
            {
                Debug.LogError("No playercontroller script on " + collider.gameObject.name);
            }

            //spawn effect here

            Destroy(gameObject);
        }
    }
}
