using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public float SwingSpeed;
    public float Damage;
    public float Cooldown;
    [SerializeField] private LayerMask m_owlLayers = default;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (m_owlLayers == (m_owlLayers | (1 << collider.gameObject.layer)))
        {
            Owl owl = collider.gameObject.GetComponent<Owl>();
            if (owl)
            {
                owl.ApplyDamage(Damage);
            }
            else
            {
                Debug.LogError("No owl script on " + owl.gameObject.name);
            }
        }else if (collider.gameObject.CompareTag("Fire"))
        {
            var fire = collider.gameObject.GetComponent<ObstacleTakeDamage>();
            if (fire)
            {
                fire.ApplyDamage(Damage);
            }
            else
            {
                Debug.LogError("No fire script on " + fire.name);
            }
        }
    }
}
