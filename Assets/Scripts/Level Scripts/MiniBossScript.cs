using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossScript : MonoBehaviour
{
    [SerializeField] private GameObject _exit;
    // Start is called before the first frame update
    
    private void OnDestroy()
    {
        var item = Instantiate(_exit, gameObject.transform.parent.transform, true);
        item.transform.localPosition = new Vector3(0,0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //for now
        if (other.gameObject.CompareTag("Player"))
        {
            Invoke(nameof(killMe), 2);
        }
    }

    private void killMe()
    {
        Destroy(gameObject);
    }
}
