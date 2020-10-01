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

    public int columns = 7;
    public int rows = 7;

    public Count obstacles = new Count(5, 15);
    public Count enemies = new Count(1, 5);

    public GameObject[] obstacleTiles;
    public GameObject doorTile;
    public GameObject[] floorTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform roomHolder;

    private List<Vector3> gridPosition = new List<Vector3>();


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void InitializeList()
    {
        gridPosition.Clear();
        for (int i = 1; i <= rows - 1; i++)
        {
            for (int j = 1; j <= columns - 1; j++)
            {
                gridPosition.Add(new Vector3(i, j, 0f));
            }
        }
    }

    void RoomSetup()
    {
        roomHolder = new GameObject("Room").transform;
        for (int i = -1; i <= rows + 1; i++)
        {
            for (int j = -1; j <= columns + 1; j++)
            {
                GameObject toInstantiate;
                if (i == -1 || i == rows+1 || j == -1 || j == columns+1)
                {
                    if (j == columns/2 || i ==  rows/2  )
                    {
                        toInstantiate = doorTile;
                    }
                    else
                    {
                        toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                    }
                }
                else
                {
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                }

                GameObject instance =
                    Instantiate(toInstantiate, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(roomHolder);
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
            Instantiate(placing, position, Quaternion.identity);
        }
    }

    public void SetupRoom(int roomNumber, int difficulty = 0)
    {
        RoomSetup();
        InitializeList();
        PlaceObjectAtRandom(obstacleTiles, obstacles.minimum, obstacles.maximum);
        int count = (int) Mathf.Log(difficulty, 2f);
        PlaceObjectAtRandom(enemyTiles, enemies.minimum + count, enemies.maximum + count);
    }
}