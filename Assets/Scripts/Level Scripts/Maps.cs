using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maps : MonoBehaviour
{

    [SerializeField] [Range(1,4)] // depending on level. Will change prefab color.
    int m_floor = 1;

    public GameObject prefabRef;

    GameObject prefabInstantiated;
    


    void Start() 
    {
        prefabInstantiated = Instantiate(prefabRef, new Vector2(0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_floor == 1) 
        {
            prefabInstantiated.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        } 
        else if(m_floor == 2)
        {

        } 
        else if (m_floor == 3)
        {

        }
        else if (m_floor == 4)
        {

        }
        Destroy(prefabInstantiated);
    }
}
