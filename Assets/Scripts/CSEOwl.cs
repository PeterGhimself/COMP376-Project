using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSEOwl : Owl
{
    [Header("StationaryOwl")]
    public GameObject projectilePrefab;

    // atomic parameters
    public float projectileDamage;
    public float projectileSpeed;
    private Rigidbody2D owlRigidBody;
    private bool moving;
    private float directionTimer; // holds timer before changing direction

    private Vector3 playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        owlRigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        moving = false;
        directionTimer = 3f;

        Physics2D.IgnoreLayerCollision(10, 10); // removes collision between enemies
        Physics2D.IgnoreLayerCollision(10, 11); // removes collision between enemies
        Physics2D.IgnoreLayerCollision(11, 11); // removes collision between enemies and their projectiles
    }

    // Update is called once per frame
    void Update()
    {
        //If not in active room. don't do anything
        if (!base.IsActive())
        {
            return;
        }
        
        directionTimer -= Time.deltaTime;

        if (moving == false)
        {
            playerPosition = player.transform.position; // store player's current position
            // head towards that position

            gameObject.GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position).normalized * moveSpeed);

            moving = true; // change state to moving
        }

        if (directionTimer < 0 && moving)
        {
            // nullify the current forces
            owlRigidBody.velocity = Vector2.zero;
            owlRigidBody.angularVelocity = 0;

            explodeProjectiles();

            directionTimer = Random.Range(2, 5); // generate a random idleTimer for the next

            moving = false;
        }
    }


    void explodeProjectiles()
    {
        // spawn 8 projectiles
        GameObject firstProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;
        GameObject secondProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;
        GameObject thirdProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;
        GameObject fourthProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;
        GameObject fifthProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;
        GameObject sixthProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;
        GameObject seventhProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;
        GameObject eighthProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;


        // project them towards 8 different directions
        firstProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 0) * projectileSpeed);
        secondProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 1).normalized * projectileSpeed);
        thirdProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1) * projectileSpeed);
        fourthProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 1).normalized * projectileSpeed);
        fifthProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 0) * projectileSpeed);
        sixthProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, -1).normalized * projectileSpeed);
        seventhProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -1) * projectileSpeed);
        eighthProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, -1).normalized * projectileSpeed);

        firstProjectile.GetComponent<ProjectileScript>().damage = projectileDamage;
        secondProjectile.GetComponent<ProjectileScript>().damage = projectileDamage;
        thirdProjectile.GetComponent<ProjectileScript>().damage = projectileDamage;
        fourthProjectile.GetComponent<ProjectileScript>().damage = projectileDamage;
        fifthProjectile.GetComponent<ProjectileScript>().damage = projectileDamage;
        sixthProjectile.GetComponent<ProjectileScript>().damage = projectileDamage;
        seventhProjectile.GetComponent<ProjectileScript>().damage = projectileDamage;
        eighthProjectile.GetComponent<ProjectileScript>().damage = projectileDamage;
    }

}
