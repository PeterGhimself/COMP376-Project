using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlinatorTurretAI : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileDamage;
    public float projectileSpeed;

    public int shootIntervalRange;
    private float shootTimer;


    // Start is called before the first frame update
    void Start()
    {
        shootIntervalRange = 10;
        shootTimer = Random.Range(4, shootIntervalRange);
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer -= Time.deltaTime;

        if(shootTimer < 0)
        {
            shootProjectile();
            shootTimer = Random.Range(4, shootIntervalRange);
        }
    }

    void shootProjectile()
    {
        GameObject bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as GameObject;
        bullet.GetComponent<ProjectileScript>().damage = projectileDamage;

        bullet.GetComponent<Rigidbody2D>().AddForce(-transform.up * projectileSpeed);

    }
}
