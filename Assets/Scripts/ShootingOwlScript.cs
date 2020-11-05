using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingOwlScript : Owl
{
    [Header("ShootingOwl")]
    public GameObject projectilePrefab;

    // atomic parameters

    public float projectileDamage;
    public float projectileSpeed;

    public float originalDirectionTimer; // change direction rate (set in seconds)
    public float originalShootTimer; // shoot rate (set in seconds)

    private Rigidbody2D owlRigidBody;

    private float directionTimer; // holds timer before changing direction
    private float shootTimer;


    // Start is called before the first frame update
    void Start()
    {
        originalDirectionTimer = 2f;
        originalShootTimer = 1f;

        owlRigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");

        directionTimer = 0f;
        shootTimer = originalShootTimer;

        Physics2D.IgnoreLayerCollision(10, 10); // removes collision between enemies
        Physics2D.IgnoreLayerCollision(10, 11); // removes collision between enemies
        Physics2D.IgnoreLayerCollision(11, 11); // removes collision between enemies and their projectiles
    }

    // Update is called once per frame
    void Update()
    {
        directionTimer -= Time.deltaTime;

        // if directionTimer runs out, call applyRandomDirection() and reset direction timer;
        if (directionTimer < 0)
        {
            applyRandomDirection();
            directionTimer = originalDirectionTimer;
        }

        shootTimer -= Time.deltaTime;
        
        // if shootTimer runs out, shoot a projectile towards the player
        if(shootTimer < 0)
        {
            GameObject bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;
            bullet.GetComponent<ProjectileScript>().damage = projectileDamage;

            bullet.GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position).normalized * projectileSpeed);

            shootTimer = originalShootTimer;
        }
    }

    void applyRandomDirection()
    {
        // nullify the current forces
        owlRigidBody.velocity = Vector2.zero;
        owlRigidBody.angularVelocity = 0;


        // generate new x and y values for the new force to be applied
        float randomX = Random.Range(-1, 2) * moveSpeed;
        float randomY = Random.Range(-1, 2) * moveSpeed;

        owlRigidBody.velocity = new Vector2(randomX, randomY);
    }
}