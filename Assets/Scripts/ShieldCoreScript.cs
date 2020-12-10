using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCoreScript : MonoBehaviour
{
    public int health;
    public GameObject bubble;

    // Start is called before the first frame update
    void Start()
    {
        health = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            bubble.SetActive(false);
            gameObject.SetActive(false);
        }
    }

}
