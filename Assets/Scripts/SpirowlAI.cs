using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpirowlAI : Owl
{
    public BoxCollider2D generalCollider;

    private Rigidbody2D owlRigidBody;

    public float originalDirectionTimer; // change direction rate (set in seconds)
    private float directionTimer; // holds timer before changing direction

    public GameObject firstWing;
    public GameObject secondWing;
    public GameObject thirdWing;
    public GameObject fourthWing;

    public float wingsRotationSpeed = 50f;
    public float wingsExpandSpeed = 1f;

    public bool moving;

    private bool wingsRotating;
    private bool wingsExpanding;

    private float wingsRotatingCooldown;
    private float wingsRotatingTimer;

    private float wingsExpandDuration;
    private float wingsExpandTimer;

    private float randomX;
    private float randomY;

    public Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        owlRigidBody = GetComponent<Rigidbody2D>();

        originalDirectionTimer = 2f;

        wingsRotating = true;
        wingsExpanding = false;
        moving = true;

        wingsRotatingCooldown = 3f;
        wingsRotatingTimer = wingsRotatingCooldown;

        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(moving)
        {
            directionTimer -= Time.deltaTime;
            // if directionTimer runs out, call applyRandomDirection() and reset direction timer;
            if (directionTimer < 0)
            {
                // generate new x and y values for the new force to be applied
                randomX = Random.Range(-5, 6);
                randomY = Random.Range(-5, 6);
                targetPosition = new Vector3(randomX + transform.position.x, randomY + transform.position.y, transform.position.z);
                directionTimer = originalDirectionTimer;
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

        wingsRotatingTimer -= Time.deltaTime;

        if(wingsRotatingTimer < 0)
        {
            wingsRotating = false;
        }


        if(wingsRotating && !wingsExpanding)
        {
            firstWing.transform.RotateAround(transform.position, Vector3.forward, wingsRotationSpeed * Time.deltaTime);
            secondWing.transform.RotateAround(transform.position, Vector3.forward, wingsRotationSpeed * Time.deltaTime);
            thirdWing.transform.RotateAround(transform.position, Vector3.forward, wingsRotationSpeed * Time.deltaTime);
            fourthWing.transform.RotateAround(transform.position, Vector3.forward, wingsRotationSpeed * Time.deltaTime);

            moving = true;
        }
        else if(!wingsRotating && !wingsExpanding)
        {
            moving = false;

            wingsExpanding = true;
            firstWing.GetComponent<WingAI>().expand();
            secondWing.GetComponent<WingAI>().expand();
            thirdWing.GetComponent<WingAI>().expand();
            fourthWing.GetComponent<WingAI>().expand();
        }
        else if (wingsExpanding)
        {
            if (firstWing.GetComponent<WingAI>().expanding == false && firstWing.GetComponent<WingAI>().retracting == false &&
                secondWing.GetComponent<WingAI>().expanding == false && secondWing.GetComponent<WingAI>().retracting == false &&
                thirdWing.GetComponent<WingAI>().expanding == false && thirdWing.GetComponent<WingAI>().retracting == false &&
                fourthWing.GetComponent<WingAI>().expanding == false && fourthWing.GetComponent<WingAI>().retracting == false)
            {
                wingsExpanding = false;
                wingsRotating = true;
                wingsRotatingTimer = wingsRotatingCooldown;
            }
        }
    }
}
