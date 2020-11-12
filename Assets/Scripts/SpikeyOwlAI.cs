using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeyOwlAI : Owl
{
    [Header("SpikeyOwl")]
    // atomic parameters
    public float originalDirectionTimer;

    private Rigidbody2D owlRigidBody;

    private float directionTimer; // holds timer before changing direction

    // Start is called before the first frame update
    void Start()
    {
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
        //If not in active room. don't do anything
        if (!base.IsActive())
        {
            return;
        }
        
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
