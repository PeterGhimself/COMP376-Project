using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTakeDamage : MonoBehaviour
{
    [SerializeField] public float maxHitPoints = 3;

    public float hitPoints;
    public Vector3 initialScale;

    // Start is called before the first frame update
    void Awake()
    {
        hitPoints = maxHitPoints;
    }

    private void Start()
    {
        initialScale = transform.localScale;
    }

    private void Update()
    {
        transform.localScale = initialScale * (hitPoints / maxHitPoints);
    }

    // Update is called once per frame
    public void ApplyDamage(float damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}