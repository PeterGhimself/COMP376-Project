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

    private bool rotating; // bool to indicate that the turret is currently rotating towards a certain angle
    private bool chargingLaser;

    private float currentLaserCooldown;

    private float randomAngle; // used to hold the angle the turret is currently rotating to
    private Quaternion originalRotation; // holds original rotation of the turret
    private Quaternion targetRotation; // used to hold the next angle the turret will rotate to

    public GameObject aimPoint; // attach an empty gameobject where you want the enemy to aim
    private Vector3 aimDirection; // holds the difference between the aimer's position adn the player's position

    private LineRenderer lineRenderer; // used to draw a line when an enemy spots a player

    private RoomManager _mRoomManager;

    // Start is called before the first frame update
    void Start()
    {
        // line rendeder set up
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.SetColors(Color.white, Color.white);

        // initial values for the turret
        range = Vector3.Distance(transform.position, aimPoint.transform.position); // default range will be the distance between the turret and its aimer

        rotating = false;
        _mRoomManager = gameObject.transform.parent.GetComponent<RoomManager>();

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
        
        if (!rotating && !chargingLaser)
        {
            //generate random angle based on the vision angle
            //(ex. if vision angle is 90, an angle from -45 and 45 can be generated
            randomAngle = Random.Range(-visionAngle / 2, (visionAngle / 2) + 1);

            // set target rotation
            targetRotation = Quaternion.Euler(0, 0, randomAngle);
            rotating = true;
        }

        if (rotating)
        {
            // rotate the turret
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // if target rotation is reached, stop rotating
            if (targetRotation == transform.rotation)
            {
                rotating = false;
            }
        }

        aimDirection = aimPoint.transform.position - transform.position;

        int mask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Obstacles")); // make raycast search for in the player layer or obstacle layer
        RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDirection, range, mask); // shoot raycast from the enemy towards its aimer (child)

        Debug.DrawRay(transform.position, aimDirection, Color.red); // used to see the raycast in the editor

        // if the collider is not null, then a player has been found (layer mask ensures that it only hits objects in the player mask)
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player") && currentLaserCooldown <= 0)
            {
                print("PLAYER DETECTED!!!");
                rotating = false;
                chargingLaser = true;
                lineRenderer.SetColors(Color.white, Color.white);
                warningLaser();
                chargeTime = 1f;
                currentLaserCooldown = laserCooldown;
            }
        }
        else if (!chargingLaser)
        {
            lineRenderer.enabled = false;
        }
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

    // shootPlayer currently just draws a line in between the player and the enemy
    void warningLaser()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, aimPoint.transform.position);
        lineRenderer.SetWidth(0.025f, 0.025f);

        lineRenderer.enabled = true;
    }

    void fireLaser()
    {
        lineRenderer.SetWidth(0.1f, 0.1f);
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
                    Debug.LogError("No playercontroller script on " + player.name);
                }
            }
        }
    }
}