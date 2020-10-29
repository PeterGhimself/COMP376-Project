using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryOwlAI : MonoBehaviour
{
    // customizable variables
    public float hitPoints;
    public float range;
    public float visionAngle;
    public float rotationSpeed;

    private bool rotating; // bool to indicate that the turret is currently rotating towards a certain angle

    private float randomAngle; // used to hold the angle the turret is currently rotating to
    private Quaternion originalRotation; // holds original rotation of the turret
    private Quaternion targetRotation; // used to hold the next angle the turret will rotate to

    public GameObject aimPoint; // attach an empty gameobject where you want the enemy to aim
    private Vector3 aimDirection; // holds the difference between the aimer's position adn the player's position

    private LineRenderer lineRenderer; // used to draw a line when an enemy spots a player

    // Start is called before the first frame update
    void Start()
    {
        // line rendeder set up
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.useWorldSpace = true;

        // initial values for the turret
        range = Vector3.Distance(transform.position, aimPoint.transform.position); // default range will be the distance between the turret and its aimer
        visionAngle = 45f;
        rotationSpeed = 5f;

        rotating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!rotating)
        {
            //generate random angle based on the vision angle
            //(ex. if vision angle is 90, an angle from -45 and 45 can be generated
            randomAngle = Random.Range(-visionAngle/2, (visionAngle/2) + 1);

            // set target rotation
            targetRotation = Quaternion.Euler(0, 0, randomAngle);
            rotating = true;
        }

        if(rotating)
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

        int mask = (1 << LayerMask.NameToLayer("Player")) | ( 1 << LayerMask.NameToLayer("Obstacles")); // make raycast search for in the player layer or obstacle layer
        RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDirection, range, mask); // shoot raycast from the enemy towards its aimer (child)

        Debug.DrawRay(transform.position, aimDirection, Color.red); // used to see the raycast in the editor

        // if the collider is not null, then a player has been found (layer mask ensures that it only hits objects in the player mask)
        if(hit.collider != null)
        {
            if(hit.collider.CompareTag("Player"))
            {
                print("PLAYER DETECTED!!!");
                shootPlayer();
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }

    }

    // shootPlayer currently just draws a line in between the player and the enemy
    void shootPlayer()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, aimPoint.transform.position);
        lineRenderer.enabled = true;
    }
}
