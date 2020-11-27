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
    
    public Count obstacles = new Count(0, 10);
    public Count enemies = new Count(1, 3);

    
    [SerializeField] [Range(1,4)] 
    int m_level = 1;

    public GameObject prefabRef;

    public GameObject[] obstacleTiles;
    public GameObject healthDrop;
    public GameObject[] floorTiles;
    public GameObject[] enemyTiles;
    private int floor;

    private float m_dropChance;
    
    private GameManager _gameManager;

    private bool spawnpoint = false;
    private bool bossRoom = false;
    private bool itemRoom = false;
    
    private bool activeRoom = false;
    //will make private later
    public int enemyCount = 0;
    
    public bool currentRoom = false;

    public MoveCameraScript m_cameraScript;

    public bool Spawnpoint
    {
        get => spawnpoint;
        set => spawnpoint = value;
    }

    public bool BossRoom
    {
        get => bossRoom;
        set => bossRoom = value;
    }

    public bool ItemRoom
    {
        get => itemRoom;
        set => itemRoom = value;
    }

    private List<Vector3> gridPosition = new List<Vector3>();

    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        floor = _gameManager.getLevel();
        m_dropChance = (1.0f + 4.0f - floor) / 10.0f;
        SetupRoom();
        m_cameraScript = Camera.main.GetComponent<MoveCameraScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (spawnpoint)
        {
            currentRoom = true;
        }
        else if (itemRoom)
        {
            PlaceItemInRoom();
        }
        else if (bossRoom)
        {
            SpawnBoss();
        }
        else
        {
            PlaceObjectAtRandom(obstacleTiles, obstacles.minimum, obstacles.maximum);
            PlaceObjectAtRandom(enemyTiles, enemies.minimum * floor, enemies.maximum * floor, true);
        }

        MapChange();
    }

    // Update is called once per frame
    void Update()
    {
        if (activeRoom && enemyCount <= 0)
        {
            activeRoom = false;
            //spawn health here. 
            if(Random.Range(0f, 1f) <= m_dropChance )
            {
                Vector3 position = RandomPosition();
                var placed = Instantiate(healthDrop, gameObject.transform, true);
                placed.transform.localPosition = position;
                
            }
        }
    }

    private void MapChange()
    {
        if(m_level == 1)
        {
            prefabRef.GetComponent<SpriteRenderer>().color = Color.red; // arctic
        }
        else if(m_level == 2)
        {
            prefabRef.GetComponent<SpriteRenderer>().color = Color.blue; // ocean
        }
        else if(m_level == 3)
        {
            prefabRef.GetComponent<SpriteRenderer>().color = Color.white; // cave (original)
        }
        else if(m_level == 4)
        {
             prefabRef.GetComponent<SpriteRenderer>().color = Color.green; // forest
        }
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

    void PlaceObjectAtRandom(GameObject[] placingArray, int min, int max, bool enemy = false)
    {

        int objectCount = Random.Range(min, max);
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 position = RandomPosition();
            GameObject placing = placingArray[Random.Range(0, placingArray.Length)];
            var placed = Instantiate(placing, gameObject.transform, true);
            placed.transform.localPosition = position;
            if (enemy)
            {
                enemyCount++;
            }
        }
        
        if (enemyCount > 0)
        {
            activeRoom = true;
        }
    }

    public void SetupRoom()
    {
        RoomSetup();
        InitializeList();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            currentRoom = true;
            var position = transform.position;
            m_cameraScript.SetDesiredPosition(position.x, position.y);
            var playerObject = other.gameObject;
            var playerPosition = playerObject.transform.position;
            Vector3 newVector = (position - playerPosition).normalized;
            playerPosition += newVector;
            playerObject.transform.position = playerPosition;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            currentRoom = false;
        }
    }
    private void PlaceItemInRoom()
    {
        Debug.Log("SpawnedItem");
        var item = Instantiate(_gameManager.RandomItem(), gameObject.transform, true);
        item.transform.localPosition = new Vector3(0,0);
    }
    
    private void SpawnBoss()
    {
        Debug.Log("SpawnedBoss");
        var item = Instantiate(_gameManager.GetBoss(), gameObject.transform, true);
        item.transform.localPosition = new Vector3(0,0);
    }

}