using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public RoomManager roomScript;

    private int floor = 6;
    // Start is called before the first frame update
    void Awake()
    {
        roomScript = GetComponent<RoomManager>();
        InitFloor();
    }

    void InitFloor()
    {
        roomScript.SetupRoom(1,floor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
