using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maps : MonoBehaviour
{

    [SerializeField] [Range(1,4)] // depending on level. Will change prefab color.
    int m_floor = 1;

    public GameObject prefabRef;
    


    void Start() 
    {
        var prefabInstantiated = Instantiate(prefabRef, new Vector2(0, 0), Quaternion.identity);

        if(m_floor == 1) 
        {
            prefabInstantiated.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white); // arctic
        } 
        else if(m_floor == 2)
        {
            prefabInstantiated.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue); // ocean

        } 
        else if (m_floor == 3)
        {
            prefabInstantiated.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black); // cave (original)
        }
        else if (m_floor == 4)
        {
            prefabInstantiated.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green); // forest
        }
        Destroy(prefabInstantiated);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
