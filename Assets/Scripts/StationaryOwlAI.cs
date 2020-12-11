using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryOwlAI : Owl
{
    [Header("StationaryOwl")]
    // customizable variables
    public float laserDamage;

    public float range;
    public float visionAngle;
    public float rotationSpeed;
    public float chargeTime;
    public float laserCooldown;

    private Rigidbody2D owlRigidBody;

    public float originalDirectionTimer; // change direction rate (set in seconds)
    private float directionTimer; // holds timer before changing direction

    private bool rotating; // bool to indicate that the turret is currently rotating towards a certain angle
    private bool chargingLaser;

    private float currentLaserCooldown;

    public GameObject aimPoint; // attach an empty gameobject where you want the enemy to aim
    private Vector3 aimDirection; // holds the difference between the aimer's position adn the player's position

    private LineRenderer lineRenderer; // used to draw a line when an enemy spots a player
    
    // Start is called before the first frame update
    void Start()
    {
        owlRigidBody = GetComponent<Rigidbody2D>();

        originalDirectionTimer = 2f;
        directionTimer = 0f;

        // line rendeder set up
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.SetColors(Color.white, Color.white);

        // initial values for the turret
        range = Vector3.Distance(transform.position, aimPoint.transform.position); // default range will be the distance between the turret and its aimer

        rotating = false;

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(k_owlFlyAnim, owlRigidBody.velocity.magnitude > 0);

        //If not in active room. don't do anything
        if (!IsActive())
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            return;
        }

        if(!chargingLaser)
            directionTimer -= Time.deltaTime;

        // if directionTimer runs out, call applyRandomDirection() and reset direction timer;
        if (directionTimer < 0)
        {
            applyRandomDirection();
            directionTimer = originalDirectionTimer;
        }

        if (!rotating && !chargingLaser)
        {
            rotating = true;
            lineRenderer.SetWidth(0.025f, 0.025f);
            lineRenderer.SetColors(Color.white, Color.white);
        }

        if (rotating)
        {
            // rotate the turret
            aimPoint.transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }

        aimDirection = aimPoint.transform.position - transform.position;

        int mask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Contact")); // make raycast search for in the player layer or obstacle layer
        RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDirection, range, mask); // shoot raycast from the enemy towards its aimer (child)

        Debug.DrawRay(transform.position, aimDirection, Color.red); // used to see the raycast in the editor

        // if the collider is not null, then a player has been found (layer mask ensures that it only hits objects in the player mask)
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player") && currentLaserCooldown <= 0)
            {
                rotating = false;
                chargingLaser = true;
                chargeTime = 1f;
                currentLaserCooldown = laserCooldown;

                // nullify the current forces
                owlRigidBody.velocity = Vector2.zero;
                owlRigidBody.angularVelocity = 0;
            }
        }
        else if (!chargingLaser)
        {
            //lineRenderer.enabled = false;
        }

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, aimPoint.transform.position);

        if (currentLaserCooldown > 0)
        {
            currentLaserCooldown -= Time.deltaTime;
        }

        if (chargingLaser)
        {
            chargeTime -= Time.deltaTime;
            if (chargeTime < 0)
            {
                fireLaser();
                currentLaserCooldown = laserCooldown;
            }
        }
    }

    void fireLaser()
    {
        lineRenderer.SetWidth(0.2f, 0.2f);
        lineRenderer.SetColors(Color.red, Color.red);
        chargingLaser = false;

        int mask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Obstacles")); // make raycast search for in the player layer or obstacle layer
        RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDirection, range, mask); // shoot raycast from the enemy towards its aimer (child)

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                PlayerController playerCtrl = hit.collider.gameObject.GetComponent<PlayerController>();
                if (playerCtrl)
                {
                    playerCtrl.DamagePlayer(laserDamage);
                }
                else
                {
                    Debug.LogError("No playercontroller script on " + hit.collider.gameObject.name);
                }
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