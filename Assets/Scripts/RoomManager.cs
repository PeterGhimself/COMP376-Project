using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int maximum;
        public int minimum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 6;
    public int rows = 6;
    public Count obstacles = new Count(5, 15);
    public Count enemies = new Count(1, 3);

    public GameObject[] obstacleTiles;
    public GameObject doorTile;
    public GameObject[] floorTiles;
    public GameObject[] enemyTiles;
    private int floor;
    private Transform roomHolder;

    private bool spawnpoint = false;

    public bool Spawnpoint
    {
        get => spawnpoint;
        set => spawnpoint = value;
    }

    private List<Vector3> gridPosition = new List<Vector3>();

    private void Awake()
    {
        // Debug.Log(transform.parent.parent);
        floor = 1;
        SetupRoom(floor);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!spawnpoint)
        {
            PlaceObjectAtRandom(obstacleTiles, obstacles.minimum, obstacles.maximum);
            PlaceObjectAtRandom(enemyTiles, enemies.minimum * floor, enemies.maximum * floor);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void InitializeList()
    {
        gridPosition.Clear();
        for (int i = -3; i <= 3; i++)
        {
            for (int j = -3; j <= 3; j++)
            {
                gridPosition.Add(new Vector3(i, j, 0f));
            }
        }
    }
    public int Floor
    {
        get => floor;
        set => floor = value;
    }

    void RoomSetup()
    {
        roomHolder = new GameObject("Room").transform;
        for (int i = -4; i <= 4; i++)
        {
            for (int j = -4; j <= 4; j++)
            {
                var toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                var instance = Instantiate(toInstantiate, gameObject.transform, true);
                instance.transform.localPosition = new Vector3(i, j, 0f);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPosition.Count);
        Vector3 randomPosition = gridPosition[randomIndex];
        gridPosition.RemoveAt(randomIndex);
        return randomPosition;
    }

    void PlaceObjectAtRandom(GameObject[] placingArray, int min, int max)
    {
        int objectCount = Random.Range(min, max);
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 position = RandomPosition();
            GameObject placing = placingArray[Random.Range(0, placingArray.Length)];
            var flr = Instantiate(placing, gameObject.transform, true);
            flr.transform.localPosition = position;
        }
    }

    public void SetupRoom(int difficulty)
    {
        RoomSetup();
        InitializeList();
    }
}