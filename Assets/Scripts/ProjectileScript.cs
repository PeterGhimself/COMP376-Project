using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float damage;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(11, 11); // removes collision between enemies and their projectiles
        Physics2D.IgnoreLayerCollision(14, 14); // projectiles ignore collision with each other
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject); // destroy the projectile if they hit something with a collider
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Rooms") && !other.gameObject.GetComponent<RoomManager>().currentRoom)
        {
            Destroy(gameObject); 
        }
    }
}
