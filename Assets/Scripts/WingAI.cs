using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WingAI : MonoBehaviour
{
    public GameObject mainBody;
    private Rigidbody2D wingRigidbody;
    private Vector3 originalPosition;
    public bool amInOriginalPosition;
    public bool expanding;
    public bool retracting;

    private float touchDamage;

    private bool invincibleToWing;
    private float invincibleToWingTimer;

    // Start is called before the first frame update
    void Start()
    {
        mainBody = transform.parent.gameObject;
        wingRigidbody = GetComponent<Rigidbody2D>();
        amInOriginalPosition = true;
        expanding = false;
        touchDamage = 1f;
        invincibleToWing = false;
        invincibleToWingTimer = 2f;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(expanding)
        {
            Vector3 tempVect = (transform.position - transform.parent.transform.position).normalized;
            wingRigidbody.MovePosition(transform.position + tempVect * mainBody.transform.GetComponent<SpirowlAI>().wingsExpandSpeed * Time.deltaTime);
        }
        if(retracting)
        {
            Vector3 tempVect = -(transform.position - originalPosition).normalized;
            wingRigidbody.MovePosition(transform.position + tempVect * mainBody.transform.GetComponent<SpirowlAI>().wingsExpandSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, originalPosition) < 0.2f)
            {
                transform.position = originalPosition;
                wingRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                retracting = false;
            }
        }

        if (invincibleToWing)
        {
            invincibleToWingTimer -= Time.deltaTime;
            Physics2D.IgnoreLayerCollision(9, 13, true); // removes collision between wing and player
            if (invincibleToWingTimer < 0)
            {
                Physics2D.IgnoreLayerCollision(9, 13, false); // enable collision between wing and player
                invincibleToWing = false;
            }
        }
    }

    public void expand()
    {
        originalPosition = transform.position;
        wingRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        expanding = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null)
        {

            if (!collision.collider.CompareTag("Player"))
            {
                if(expanding == true || retracting == true)
                {
                    wingRigidbody.velocity = Vector3.zero;
                    wingRigidbody.angularVelocity = 0;
                    expanding = false;
                    retracting = true;
                }
                else
                {
                    Transform currentRoom = mainBody.transform.parent;
                    mainBody.GetComponent<SpirowlAI>().targetPosition = new Vector3(currentRoom.position.x, currentRoom.position.y, mainBody.GetComponent<SpirowlAI>().targetPosition.z);
                    
                }
            }
            else
            {
                PlayerController playerCtrl = collision.gameObject.GetComponent<PlayerController>();
                playerCtrl.DamagePlayer(touchDamage);
                invincibleToWing = true;
            }

        }
    }
}
