using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Obstacle
{
    // Start is called before the first frame update
    [SerializeField] public float Damage;
    [SerializeField] private LayerMask m_owlLayers = default;

    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (m_owlLayers == (m_owlLayers | (1 << other.gameObject.layer)))
        {
            Owl owl = other.gameObject.GetComponent<Owl>();
            if (owl)
            {
                owl.ApplyDamage(Damage);
            }
            else
            {
                Debug.LogError("No owl script on " + owl.gameObject.name);
            }
        }else if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player)
            {
                player.DamagePlayer(Damage);
            }
            else
            {
                Debug.LogError("No player script on " + player.gameObject.name);
            }
        }
    }
}
