using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class FloorManager : MonoBehaviour
{
    [SerializeField] int floor = 1;
    [SerializeField] private GameObject[] rooms;
    private int[,] _map = new int [12, 12];
    private int _roomBank;

    private Queue<RoomSpot> incomplete = new Queue<RoomSpot>();
    private List<RoomSpot> done = new List<RoomSpot>();
    private int roomNumber = 0;

    private class RoomSpot
    {
        private int _x;
        private int _y;

        public int GETRoomType(int[,] map)
        {
            int type = 0;
            if (_y - 1 >= 0 && map[_y - 1, _x] > 0)
            {
                type += 1;
            }

            if (_x - 1 >= 0 && map[_y, _x - 1] > 0)
            {
                type += 2;
            }

            if (_x + 1 < map.GetLength(1) && map[_y, _x + 1] > 0)
            {
                type += 8;
            }

            if (_y + 1 < map.GetLength(0) && map[_y + 1, _x] > 0)
            {
                type += 4;
            }

            return type;
        }

        public int X
        {
            get => _x;
            set => _x = value;
        }

        public int Y
        {
            get => _y;
            set => _y = value;
        }

        public RoomSpot(int y, int x)
        {
            _x = x;
            _y = y;
        }
    }

    private int GetRoomNumber()
    {
        roomNumber++;
        return roomNumber;
    }

    void Start()
    {
        InitializeEmptyMap();
        _roomBank = 7 + 3 * floor;
        RoomSpot temp = new RoomSpot(5, 5);
        incomplete.Enqueue(temp);
        GenerateMap();
        GenerateBossAndItemRoom();
        InstantiateRooms();
        // PrintMap();
    }

    private void InstantiateRooms()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = height * Screen.width / Screen.height;
        bool spawn = true;
        foreach (var room in done)
        {
            GameObject created;
            if (_map[room.Y, room.X] == 666 || _map[room.Y, room.X] == 420)
            {
                created = Instantiate(rooms[0],
                    new Vector3((room.X - 5) * width, (room.Y - 5) * height), Quaternion.identity);
            }
            else
            {
                created = Instantiate(rooms[room.GETRoomType(_map)],
                    new Vector3((room.X - 5) * width, (room.Y - 5) * height), Quaternion.identity);
                created.GetComponent<RoomManager>().Floor = floor;
                if (spawn)
                {
                    spawn = !spawn;
                    created.GetComponent<RoomManager>().Spawnpoint = true;
                }

            }
        
            created.transform.localScale = new Vector3(width / 10, height / 10);
        }
    }

    public int Floor
    {
        get => floor;
        set => floor = value;
    }

    private void GenerateBossAndItemRoom()
    {
        bool boss = false;
        RoomSpot checker = done[done.Count - 1];
        for (int i = done.Count - 1; i >= 0; i--)
        {
            var found = false;
            var failed = false;
            checker = done[i];
            if (!boss)
            {
                if (checker.Y - 1 >= 0 && _map[checker.Y - 1, checker.X] > 0)
                {
                    if (found)
                    {
                        failed = true;
                    }

                    found = true;
                }

                if (checker.X - 1 >= 0 && _map[checker.Y, checker.X - 1] > 0)
                {
                    if (found)
                    {
                        failed = true;
                    }

                    found = true;
                }

                if (checker.X + 1 < _map.GetLength(1) && _map[checker.Y, checker.X + 1] > 0)
                {
                    if (found)
                    {
                        failed = true;
                    }

                    found = true;
                }

                if (checker.Y + 1 < _map.GetLength(0) && _map[checker.Y + 1, checker.X] > 0)
                {
                    if (found)
                    {
                        failed = true;
                    }

                    found = true;
                }

                if (!failed)
                {
                    boss = true;
                    _map[checker.Y, checker.X] = 666;
                }
            }
            else
            {
                if (checker.Y - 1 >= 0 && _map[checker.Y - 1, checker.X] > 0)
                {
                    if (found)
                    {
                        failed = true;
                    }

                    found = true;
                }

                if (checker.X - 1 >= 0 && _map[checker.Y, checker.X - 1] > 0)
                {
                    if (found)
                    {
                        failed = true;
                    }

                    found = true;
                }

                if (checker.X + 1 < _map.GetLength(1) && _map[checker.Y, checker.X + 1] > 0)
                {
                    if (found)
                    {
                        failed = true;
                    }

                    found = true;
                }

                if (checker.Y + 1 < _map.GetLength(0) && _map[checker.Y + 1, checker.X] > 0)
                {
                    if (found)
                    {
                        failed = true;
                    }

                    found = true;
                }

                if (!failed)
                {
                    _map[checker.Y, checker.X] = 420;
                    break;
                }
            }
        }
    }

    private void GenerateMap()
    {
        int totalRooms = _roomBank;
        RoomSpot currentRoom = incomplete.Dequeue();
        int[] alot = {2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4};
        int[] middle = {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 3};
        int[] low = {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2};

        int creating = alot[Random.Range(0, alot.Length - 1)];
        GenerateAdjacent(currentRoom, creating);
        _roomBank -= creating;

        while (incomplete.Count > 0)
        {
            currentRoom = incomplete.Dequeue();
            if (_roomBank > totalRooms * 3 / 4 && _roomBank >= 3)
            {
                creating = middle[Random.Range(0, middle.Length - 1)];
                GenerateAdjacent(currentRoom, creating);
                _roomBank -= creating;
            }
            else if (_roomBank > totalRooms / 4 && _roomBank >= 2)
            {
                creating = low[Random.Range(0, low.Length - 1)];
                GenerateAdjacent(currentRoom, creating);
                _roomBank -= creating;
            }
            else if (_roomBank > 0)
            {
                GenerateAdjacent(currentRoom, 1);
                _roomBank -= 1;
            }
            else
            {
                done.Add(currentRoom);
            }
        }
    }

    private void InitializeEmptyMap()
    {
        for (var row = 0; row <= 11; row++)
        {
            for (var col = 0; col <= 11; col++)
            {
                if (row == 5 && col == 5)
                {
                    _map[row, col] = GetRoomNumber();
                }
                else
                {
                    _map[row, col] = 0;
                }
            }
        }
    }

    public int[,] getMap()
    {
        return _map;
    }

    public int getMapSpot(int row, int col)
    {
        return _map[row, col];
    }

    public void SetMapSpot(int row, int col, int value)
    {
        _map[row, col] = value;
    }

    private void GenerateAdjacent(RoomSpot room, int rooms)
    {
        var row = room.Y;
        var col = room.X;
        int[] directions = new int[4] {0, 1, 2, 3};
        for (int i = 0; i < 3; i++)
        {
            int swapping = Random.Range(i, 4);
            int tempA = directions[i];
            directions[i] = directions[swapping];
            directions[swapping] = tempA;
        }

        RoomSpot newRoom;

        foreach (var direction in directions)
        {
            if (direction == 0 && row - 1 >= 0 && _map[row - 1, col] == 0)
            {
                if (rooms > 0)
                {
                    newRoom = new RoomSpot(row - 1, col);
                    incomplete.Enqueue(newRoom);
                    _map[row - 1, col] = GetRoomNumber();
                    rooms--;
                }
                else
                {
                    _map[row - 1, col] = -1;
                }
            }
            else if (direction == 1 && col - 1 >= 0 && _map[row, col - 1] == 0)
            {
                if (rooms > 0)
                {
                    newRoom = new RoomSpot(row, col - 1);
                    incomplete.Enqueue(newRoom);
                    _map[row, col - 1] = GetRoomNumber();
                    rooms--;
                }
                else
                {
                    _map[row, col - 1] = -1;
                }
            }
            else if (direction == 2 && col + 1 < _map.GetLength(1) && _map[row, col + 1] == 0)
            {
                if (rooms > 0)
                {
                    newRoom = new RoomSpot(row, col + 1);
                    incomplete.Enqueue(newRoom);
                    _map[row, col + 1] = GetRoomNumber();
                    rooms--;
                }
                else
                {
                    _map[row, col + 1] = -1;
                }
            }
            else if (direction == 3 && row + 1 < _map.GetLength(0) && _map[row + 1, col] == 0)
            {
                if (rooms > 0)
                {
                    newRoom = new RoomSpot(row + 1, col);
                    incomplete.Enqueue(newRoom);
                    _map[row + 1, col] = GetRoomNumber();
                    rooms--;
                }
                else
                {
                    _map[row + 1, col] = -1;
                }
            }
        }

        done.Add(room);
    }

    private void PrintMap()
    {
        string stringRow;
        bool triggerd;
        for (int row = 11; row >= 0; row--)
        {
            stringRow = "";
            triggerd = false;
            for (int col = 0; col < 12; col++)
            {
                if (_map[row, col] != 0)
                {
                    triggerd = true;
                }

                stringRow += $"{_map[row, col]}\t";
            }

            if (triggerd)
            {
                Debug.Log(stringRow);
            }
        }
    }
}