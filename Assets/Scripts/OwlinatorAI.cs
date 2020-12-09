using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlinatorAI : Owl
{
    public GameObject bubble;

    private Rigidbody2D owlRigidbody;

    public int chargeCooldown = 5;
    private float chargeTimer;

    private Vector3 originalPosition;

    private Vector3 targetPosition;

    private bool charging;
    private bool returning;

    private float bubbleVulnerableTimer;
    private float currentBubbleVulnerableTimer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        owlRigidbody = GetComponent<Rigidbody2D>();
        chargeTimer = 2; //Random.Range(chargeCooldown, chargeCooldown + 3);
        charging = false;
        returning = false;
        originalPosition = transform.position;
        Physics2D.IgnoreLayerCollision(10, 11); // removes collision between enemies
        bubbleVulnerableTimer = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        chargeTimer -= Time.deltaTime;
        if (chargeTimer < 0 && !charging)
        {
            targetPosition = player.transform.position;
            charging = true;
        }

        if (charging)
        {
            if (!returning)
            {
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    if(!bubble.GetComponent<OwlinatorBubbleScript>().bubbleVulnerable)
                    {
                        bubble.GetComponent<OwlinatorBubbleScript>().bubbleVulnerable = true;
                        currentBubbleVulnerableTimer = bubbleVulnerableTimer;
                    }
                    else
                    {
                        print(currentBubbleVulnerableTimer);
                        currentBubbleVulnerableTimer -= Time.deltaTime;
                        if(currentBubbleVulnerableTimer < 0)
                        {
                            bubble.GetComponent<OwlinatorBubbleScript>().bubbleVulnerable = false;
                            returning = true;
                        }
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, originalPosition) < 0.1f)
                {
                    returning = false;
                    charging = false;
                    chargeTimer = Random.Range(chargeCooldown, chargeCooldown + 3);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveSpeed * Time.deltaTime);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null)
        {
            if (!collision.collider.CompareTag("Player"))
            {
                returning = true;
            }
        }
    }
    
    private void OnDestroy()
    {
    }
    
}
