using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingOwlScript : MonoBehaviour
{
    public GameObject projectilePrefab;

    // atomic parameters
    public float hitPoints;
    public float projectileSpeed;
    public float moveSpeed;
    public float originalDirectionTimer; // change direction rate (set in seconds)
    public float originalShootTimer; // shoot rate (set in seconds)

    private Rigidbody2D owlRigidBody;
    private GameObject player;

    private float directionTimer; // holds timer before changing direction
    private float shootTimer;

    private RoomManager _mRoomManager;

    // Start is called before the first frame update
    void Start()
    {
        hitPoints = 10f;
        projectileSpeed = 60f;
        moveSpeed = 0.5f;
        originalDirectionTimer = 2f;
        originalShootTimer = 1f;

        owlRigidBody = GetComponent<Rigidbody2D>();
        _mRoomManager = gameObject.transform.parent.GetComponent<RoomManager>();
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
        if (_mRoomManager.currentRoom)
        {
            // rotate the enemy towards the player
            Vector3 relativePos = player.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            rotation.x = transform.rotation.x;
            rotation.y = transform.rotation.y;
            transform.rotation = rotation;

            directionTimer -= Time.deltaTime;

            // if directionTimer runs out, call applyRandomDirection() and reset direction timer;
            if (directionTimer < 0)
            {
                applyRandomDirection();
                directionTimer = originalDirectionTimer;
            }

            shootTimer -= Time.deltaTime;

            // if shootTimer runs out, shoot a projectile towards the player
            if (shootTimer < 0)
            {
                GameObject bullet =
                    Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;
                if (player.transform.position.x >= transform.position.x)
                {
                    bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * projectileSpeed);
                }
                else
                {
                    bullet.GetComponent<Rigidbody2D>().AddForce(-transform.right * projectileSpeed);
                }

                shootTimer = originalShootTimer;
            }
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