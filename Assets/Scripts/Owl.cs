using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owl : MonoBehaviour
{
    [Header("Owl")]
    public float hitPoints;
    public float moveSpeed;
    public float touchDamage;

    protected GameObject player = default;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject == player)
        {
            PlayerController playerCtrl = collision.gameObject.GetComponent<PlayerController>();
            if (playerCtrl)
            {
                playerCtrl.DamagePlayer(touchDamage);
            }
            else
            {
                Debug.LogError("No playercontroller script on " + player.name);
            }
        }
    }
}
