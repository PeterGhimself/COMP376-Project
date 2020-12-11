using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlinatorBubbleScript : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite vulnerableSprite;

    public int bubbleHealth;
    public int currentBubbleHealth;

    public bool bubbleVulnerable;

    public bool stunned;
    public float originalStunLength;
    public float stunLength;


    void Start()
    {
        currentBubbleHealth = bubbleHealth;
        bubbleVulnerable = false;
        stunned = false;
    }

    void Update()
    {
        if(bubbleVulnerable)
            GetComponent<SpriteRenderer>().sprite = vulnerableSprite;
        else
            GetComponent<SpriteRenderer>().sprite = defaultSprite;

        if(stunned)
        {
            
            stunLength -= Time.deltaTime;

            // stun rotation logic

            if(stunLength >= originalStunLength * 0.75)    
                transform.parent.transform.Rotate(new Vector3(0, 0, 20) * Time.deltaTime);
            else if (stunLength >= originalStunLength * 0.25)
                transform.parent.transform.Rotate(new Vector3(0, 0, -20) * Time.deltaTime);
            else if (stunLength >= 0)
                transform.parent.transform.Rotate(new Vector3(0, 0, 20) * Time.deltaTime);

            if (stunLength < 0)
            {
                transform.parent.transform.rotation = Quaternion.identity;
                stunned = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != null)
        {

            if (collision.collider.CompareTag("PlayerProjectile"))
            {
                Destroy(collision.collider.gameObject);
            }

            if (collision.collider.CompareTag("Player"))
            {
                PlayerController playerCtrl = collision.gameObject.GetComponent<PlayerController>();
                if (playerCtrl)
                {
                    playerCtrl.DamagePlayer(transform.parent.GetComponent<OwlinatorAI>().touchDamage);
                }
            }
        }
    }
}
