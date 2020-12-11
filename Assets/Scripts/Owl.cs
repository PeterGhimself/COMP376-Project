using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owl : MonoBehaviour
{
    [Header("Owl")]
    public float hitPoints;
    public float moveSpeed;
    public float touchDamage;
    public LayerMask playerLayer;

    protected GameObject player = default;

    protected Animator animator = default;

    private const string k_owlHitAnim = "OwlHit";
    protected const string k_owlFlyAnim = "Moving";

    private RoomManager _mRoomManager;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
    }

    protected void Start()
    {
        _mRoomManager = gameObject.transform.parent.GetComponent<RoomManager>();
        Physics2D.IgnoreLayerCollision(10, 10); // removes collision between enemies
        Physics2D.IgnoreLayerCollision(10, 11); // removes collision between enemies
        Physics2D.IgnoreLayerCollision(11, 11); // removes collision between enemies and their projectiles
        Physics2D.IgnoreLayerCollision(10, 13); 
        Physics2D.IgnoreLayerCollision(11, 13);
        Physics2D.IgnoreLayerCollision(10, 14);
    }

    public void ApplyDamage(float damage)
    {
        hitPoints -= damage;

        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        }

        animator.SetTrigger(k_owlHitAnim);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (playerLayer == (playerLayer | (1 << collision.gameObject.layer)))
        {
            PlayerController playerCtrl = collision.gameObject.GetComponent<PlayerController>();
            if (playerCtrl)
            {
                playerCtrl.DamagePlayer(touchDamage);
            }
            else
            {
                Debug.LogError("No playercontroller script on " + collision.gameObject.name);
            }
        }
    }

    public bool IsActive()
    {
        return _mRoomManager.currentRoom;
    }

    private void OnDestroy()
    {
        _mRoomManager.enemyCount--;
    }
}
