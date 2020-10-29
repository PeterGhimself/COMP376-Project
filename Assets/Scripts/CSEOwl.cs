using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSEOwl : MonoBehaviour
{
    public GameObject projectilePrefab;

    // atomic parameters
    public float hitPoints;
    public float moveSpeed;
    public float projectileSpeed;

    private Rigidbody2D owlRigidBody;
    private GameObject player;
    private bool moving;
    private float directionTimer; // holds timer before changing direction

    private Vector3 playerPosition;

    // Start is called before the first frame update
    void Start()
    {
        hitPoints = 10f;
        moveSpeed = 25f;
        projectileSpeed = 60f;

        owlRigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        moving = false;
        directionTimer = 3f;

        Physics2D.IgnoreLayerCollision(12, 12); // removes collision between enemies
        Physics2D.IgnoreLayerCollision(12, 14); // removes collision between enemies and their projectiles
    }

    // Update is called once per frame
    void Update()
    {
        // rotate towards player
        Vector3 relativePos = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        rotation.x = transform.rotation.x;
        rotation.y = transform.rotation.y;
        transform.rotation = rotation;

        directionTimer -= Time.deltaTime;

        if (moving == false)
        {
            playerPosition = player.transform.position; // store player's current position
            // head towards that position
            if (player.transform.position.x >= transform.position.x)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * moveSpeed);
            }
            else
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(-transform.right * moveSpeed);
            }

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
    }

}
