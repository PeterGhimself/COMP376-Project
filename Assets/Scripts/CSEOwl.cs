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
    private bool stopped;
    private float directionTimer; // holds timer before changing direction
    private float stopTimer;

    private Vector3 playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        owlRigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        moving = false;
        stopped = true;
        directionTimer = 3f;
        stopTimer = 1f;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //If not in active room. don't do anything
        if (!IsActive())
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            return;
        }
        
        directionTimer -= Time.deltaTime;

        if (!moving && stopped)
        {
            playerPosition = player.transform.position; // store player's current position
            // head towards that position

            gameObject.GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position).normalized * moveSpeed);

            stopped = false;
            moving = true; // change state to moving
        }

        if (directionTimer < 0 && moving)
        {
            // nullify the current forces
            owlRigidBody.velocity = Vector2.zero;
            owlRigidBody.angularVelocity = 0;
            stopTimer = 1f;
            stopped = true;
        }

        if(stopped && moving)
        {
            explodeProjectiles();

            directionTimer = Random.Range(2, 5); // generate a random idleTimer for the next

            moving = false;
        }

    }

    //maybe try using a loop for this :)
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
        firstProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 0) * projectileSpeed, ForceMode2D.Impulse);
        secondProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 1).normalized * projectileSpeed, ForceMode2D.Impulse);
        thirdProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1) * projectileSpeed, ForceMode2D.Impulse);
        fourthProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 1).normalized * projectileSpeed, ForceMode2D.Impulse);
        fifthProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 0) * projectileSpeed, ForceMode2D.Impulse);
        sixthProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, -1).normalized * projectileSpeed, ForceMode2D.Impulse);
        seventhProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -1) * projectileSpeed, ForceMode2D.Impulse);
        eighthProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, -1).normalized * projectileSpeed, ForceMode2D.Impulse);

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
