using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public float SwingSpeed;
    public float Damage;
    public float Cooldown;
    [SerializeField] private LayerMask m_owlLayers = default;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_owlLayers == (m_owlLayers | (1 << collision.gameObject.layer)))
        {
            Owl owl = collision.gameObject.GetComponent<Owl>();
            if (owl)
            {
                owl.ApplyDamage(Damage);
            }
            else
            {
                Debug.LogError("No owl script on " + owl.gameObject.name);
            }
        }
    }
}
