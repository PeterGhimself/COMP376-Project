using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public RoomManager roomScript;

    [SerializeField]
    int floor = 6;

    void Awake()
    {
        roomScript = GetComponent<RoomManager>();
        InitFloor();
    }

    void InitFloor()
    {
        roomScript.SetupRoom(1,floor);
    }

    public void InitFloor(FloorInfo floorInfo)
    {
        //called by game manager. to be expanded with floor generation
        roomScript.SetupRoom(floorInfo.Room, floorInfo.Floor);
    }
}
