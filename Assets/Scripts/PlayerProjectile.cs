using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private LayerMask m_enemyLayers;
    [SerializeField] private float m_damage = 1;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_enemyLayers == (m_enemyLayers | (1 << collision.gameObject.layer)))
        {
            Owl owl = collision.gameObject.GetComponent<Owl>();
            if (owl)
            {
                owl.ApplyDamage(m_damage);
            }
            else
            {
                Debug.LogError("No owl script on " + owl.name);
            }
        }

        Destroy(gameObject);
    }


    public void IncreaseDamage(float damage)
    {
        m_damage += damage;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    { 
        if (other.gameObject.CompareTag("Fire"))
        {
            var fire = other.gameObject.GetComponent<ObstacleTakeDamage>();
            if (fire)
            {
                fire.ApplyDamage(m_damage);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("No owl script on " + fire.name);
            }
        }
    }
}
