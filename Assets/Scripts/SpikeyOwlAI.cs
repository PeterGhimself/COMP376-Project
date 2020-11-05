using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeyOwlAI : MonoBehaviour
{
    // atomic parameters
    public float hitPoints;
    public float moveSpeed;
    public float touchDamage;

    public float originalDirectionTimer;

    private Rigidbody2D owlRigidBody;
    private GameObject player;

    private float directionTimer; // holds timer before changing direction

    // Start is called before the first frame update
    void Start()
    {
        hitPoints = 10f;
        moveSpeed = 25f;
        touchDamage = 5f;
        originalDirectionTimer = 1f;

        owlRigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");

        directionTimer = 0f;

        Physics2D.IgnoreLayerCollision(10, 10); // removes collision between enemies
        Physics2D.IgnoreLayerCollision(10, 11); // removes collision between enemies and their projectiles
        Physics2D.IgnoreLayerCollision(11, 11); // removes collision between projectiles
    }

    // Update is called once per frame
    void Update()
    {

        directionTimer -= Time.deltaTime;

        // charge towards player location after the direction timer runs out
        if (directionTimer < 0)
        {
            // nullify previous forces
            owlRigidBody.velocity = Vector2.zero;
            owlRigidBody.angularVelocity = 0;

            // apply new force towards player
            gameObject.GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position).normalized * moveSpeed);


            directionTimer = originalDirectionTimer;
        }
    }

}
