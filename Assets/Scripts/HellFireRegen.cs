using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellFireRegen : MonoBehaviour
{
    private ObstacleTakeDamage obstacleScript;
    private float deltaDamaged = 0;
    [SerializeField]private float regenTime = 2.5f;
    // Start is called before the first frame update
    void Awake()
    {
        obstacleScript = gameObject.GetComponent<ObstacleTakeDamage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Abs(obstacleScript.hitPoints - obstacleScript.maxHitPoints) > 0)
        {
            deltaDamaged += Time.deltaTime;
        }
        else
        {
            deltaDamaged = 0;
        }

        if (deltaDamaged >= regenTime)
        {
            obstacleScript.hitPoints += 1;
            deltaDamaged = 0;
        }
    }
}
