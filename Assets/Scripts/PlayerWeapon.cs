using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public float SwingSpeed;
    public float Cooldown;
    public float KnockbackForce;

    [SerializeField] private float m_damage = default;
    [SerializeField] private LayerMask m_owlLayers = default;

    private float initDamage = 0;

    private void Start()
    {
        initDamage = m_damage;
    }

    public float GetDamage() {
        return this.m_damage;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (m_owlLayers == (m_owlLayers | (1 << collider.gameObject.layer)))
        {
            Owl owl = collider.gameObject.GetComponent<Owl>();
            if (owl)
            {
                owl.ApplyDamage(m_damage);
            }
            else
            {
                Debug.LogError("No owl script on " + collider.gameObject.name);
            }
        }else if (collider.gameObject.CompareTag("Fire"))
        {
            var fire = collider.gameObject.GetComponent<ObstacleTakeDamage>();
            if (fire)
            {
                fire.ApplyDamage(m_damage);
            }
            else
            {
                Debug.LogError("No fire script on " + collider.gameObject.name);
            }
        }
        else if(collider.gameObject.CompareTag("OwlinatorBubble"))
        {
            if(!collider.gameObject.GetComponent<OwlinatorBubbleScript>().stunned && collider.gameObject.GetComponent<OwlinatorBubbleScript>().bubbleVulnerable)
            {
                collider.gameObject.GetComponent<OwlinatorBubbleScript>().stunned = true;
                collider.gameObject.GetComponent<OwlinatorBubbleScript>().stunLength = 3f; // hardcoded stun length
                collider.gameObject.GetComponent<OwlinatorBubbleScript>().originalStunLength = 3f; // hardcoded stun length
            }

        }
        else if(collider.gameObject.CompareTag("ShieldCore"))
        {
            collider.gameObject.GetComponent<ShieldCoreScript>().health -= (int)m_damage;
            collider.gameObject.GetComponent<Animator>().SetTrigger("ShieldCoreHit");
        }
    }

    public void IncreaseByDamageMod(float damageMod)
    {
        m_damage = initDamage + damageMod;
    }
}
