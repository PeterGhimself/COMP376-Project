using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeyOwlAI : MonoBehaviour
{
    // atomic parameters
    public float hitPoints;
    public float moveSpeed;
    public float originalDirectionTimer;

    private Rigidbody2D owlRigidBody;
    private GameObject player;

    private float directionTimer; // holds timer before changing direction

    // Start is called before the first frame update
    void Start()
    {
        hitPoints = 10f;
        moveSpeed = 25f;
        originalDirectionTimer = 1f;

        owlRigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");

        directionTimer = 0f;

        Physics2D.IgnoreLayerCollision(10, 10); // removes collision between enemies
        Physics2D.IgnoreLayerCollision(10, 11); // removes collision between enemies
        Physics2D.IgnoreLayerCollision(11, 11); // removes collision between enemies and their projectiles
    }

    // Update is called once per frame
    void Update()
    {
        // rotate the enemy towards player
        Vector3 relativePos = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        rotation.x = transform.rotation.x;
        rotation.y = transform.rotation.y;
        transform.rotation = rotation;

        directionTimer -= Time.deltaTime;

        // charge towards player location after the direction timer runs out
        if (directionTimer < 0)
        {
            // nullify previous forces
            owlRigidBody.velocity = Vector2.zero;
            owlRigidBody.angularVelocity = 0;

            // apply new force towards player
            if (player.transform.position.x >= transform.position.x)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * moveSpeed);
            }
            else
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(-transform.right * moveSpeed);
            }
            directionTimer = originalDirectionTimer;
        }
    }

}
