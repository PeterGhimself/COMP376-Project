using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    //Will be used if ever we need something for all obstacles
    private RoomManager _mRoomManager;

    // Start is called before the first frame update
    protected void Start()
    {
        _mRoomManager = gameObject.transform.parent.GetComponent<RoomManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
