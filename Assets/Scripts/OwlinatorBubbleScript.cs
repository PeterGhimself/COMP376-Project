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

    void Start()
    {
        currentBubbleHealth = bubbleHealth;
        bubbleVulnerable = false;
    }

    void Update()
    {
        if(bubbleVulnerable)
            GetComponent<SpriteRenderer>().sprite = vulnerableSprite;
        else
            GetComponent<SpriteRenderer>().sprite = defaultSprite;
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
