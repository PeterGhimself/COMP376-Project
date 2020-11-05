using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D player2DRigidBody;

    public float health;
    public float playerSpeed;
    public float playerDrag; // "knockback resistance"

    void Start()
    {
        health = 50f;
        playerSpeed = 10f;
        playerDrag = 5f;
        player2DRigidBody.drag = playerDrag; // set linear drag to 5
    }

    void Update()
    {
        if(health <= 0)
        {
            print("Game Over");
        }

    }

   
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "EnemyProjectile")
        {
            health -= collision.gameObject.GetComponent<ProjectileScript>().damage;
        }

        if (collision.gameObject.tag == "SpikeyOwl")
        {
            health -= collision.gameObject.GetComponent<SpikeyOwlAI>().touchDamage;
        }
    }
}
