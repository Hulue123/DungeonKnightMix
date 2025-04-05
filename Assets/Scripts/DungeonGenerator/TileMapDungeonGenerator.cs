using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDungeonGenerator : MonoBehaviour
{
    [Header("Tilemap References")]
    public Tilemap groundTilemap;
    public Tilemap wallTilemap;
    public Tilemap doorTilemap;

    [Header("Tile Assets")]
    public TileBase wallTile;
    public TileBase doorTile;
    public TileBase floorTile;


    [Header("Dungeon Settings")]
    public Vector2Int size = new Vector2Int(5, 5); // 房间数量
    public int roomSize = 3; // 每个房间的大小(3x3)
    public int startPos = 0;

    private GameObject player;


    [Header("Enemy Settings")]
    public GameObject enemyPrefab; // 敌人预制体
    public int minEnemiesPerRoom = 1;
    public int maxEnemiesPerRoom = 3;
    public Transform enemyContainer; // 用于组织生成敌人的父物体

    [System.Serializable]
    public class RoomData
    {
        public Vector3Int position;
        public List<GameObject> enemies;
        public bool isActive;
        public List<Vector3Int> doorPositions;
    }

    private Dictionary<Vector3Int, RoomData> roomDataDict = new Dictionary<Vector3Int, RoomData>();
    private Vector3Int currentPlayerRoom;

    private List<Cell> board;

    private class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4]; // 0-Up, 1-Down, 2-Right, 3-Left
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        ClearDungeon();
        GenerateMaze();
        PaintDungeon();
    }

    void ClearDungeon()
    {
        groundTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
        doorTilemap.ClearAllTiles();
        board = new List<Cell>();
    }

    void GenerateMaze()
    {
        // Initialize cells
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = startPos;
        Stack<int> path = new Stack<int>();
        int k = 0;

        while (k < 1000)
        {
            k++;
            board[currentCell].visited = true;

            if (currentCell == board.Count - 1)
                break;

            List<int> neighbors = CheckNeighbors(currentCell);
            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                    break;
                else
                    currentCell = path.Pop();
            }
            else
            {
                path.Push(currentCell);
                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > currentCell)
                {
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true; // Right
                        board[newCell].status[3] = true;    // Left
                    }
                    else
                    {
                        board[currentCell].status[1] = true; // Down
                        board[newCell].status[0] = true;    // Up
                    }
                }
                else
                {
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true; // Left
                        board[newCell].status[2] = true;    // Right
                    }
                    else
                    {
                        board[currentCell].status[0] = true; // Up
                        board[newCell].status[1] = true;     // Down
                    }
                }
                currentCell = newCell;
            }
        }
    }

    void PaintDungeon()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Vector3Int roomOrigin = new Vector3Int(
                    i * (roomSize + 1),
                    -j * (roomSize + 1),
                    0);

                Cell currentCell = board[(i + j * size.x)];

                if (currentCell.visited)
                {
                    // 初始化房间数据
                    var roomData = new RoomData()
                    {
                        position = roomOrigin,
                        enemies = new List<GameObject>(),
                        isActive = false,
                        doorPositions = new List<Vector3Int>()
                    };
                    roomDataDict.Add(roomOrigin, roomData);

                    FillRoomWithFloor(roomOrigin);
                    GenerateWallsAndDoors(roomOrigin, currentCell);


                    // 生成敌人
                    SpawnEnemies(roomOrigin, roomData);

                }
                else
                {
                    FillRoomWithWalls(roomOrigin);
                }
            }
        }
    }

    void FillRoomWithFloor(Vector3Int origin)
    {
        for (int x = 0; x < roomSize; x++)
        {
            for (int y = 0; y < roomSize; y++)
            {
                groundTilemap.SetTile(origin + new Vector3Int(x, -y, 0), floorTile);
            }
        }
    }

    void GenerateWallsAndDoors(Vector3Int origin, Cell cell)
    {
        // 上墙壁/门
        GenerateWallSection(origin, cell.status[0], Direction.Up);
        // 下墙壁/门
        GenerateWallSection(origin, cell.status[1], Direction.Down);
        // 右墙壁/门
        GenerateWallSection(origin, cell.status[2], Direction.Right);
        // 左墙壁/门
        GenerateWallSection(origin, cell.status[3], Direction.Left);
    }

    enum Direction { Up, Down, Right, Left }

    void GenerateWallSection(Vector3Int origin, bool hasDoor, Direction direction)
    {
        int doorCenter = roomSize / 2; // 门居中

        for (int i = 0; i <= roomSize; i++)
        {
            Vector3Int wallPos = GetWallPosition(origin, i, direction);

            // 如果有门且是中间位置，则放置门
            if (hasDoor && Mathf.Abs(i - doorCenter) <= 1)
            {
                doorTilemap.SetTile(wallPos, doorTile);

                // 记录所有三个门格位置
                roomDataDict[origin].doorPositions.Add(wallPos);

            }
            else
            {
                wallTilemap.SetTile(wallPos, wallTile);
            }
        }
    }

    Vector3Int GetWallPosition(Vector3Int origin, int index, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return origin + new Vector3Int(index, 1, 0);
            case Direction.Down:
                return origin + new Vector3Int(index, -roomSize, 0);
            case Direction.Right:
                return origin + new Vector3Int(roomSize, -index, 0);
            case Direction.Left:
                return origin + new Vector3Int(-1, -index, 0);
            default:
                return origin;
        }
    }
    void FillRoomWithWalls(Vector3Int origin)
    {
        for (int x = -1; x <= roomSize; x++)
        {
            for (int y = -1; y <= roomSize; y++)
            {
                

                wallTilemap.SetTile(origin + new Vector3Int(x, -y, 0), wallTile);
            }
        }
    }

    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        // Up
        if (cell - size.x >= 0 && !board[cell - size.x].visited)
            neighbors.Add(cell - size.x);

        // Down
        if (cell + size.x < board.Count && !board[cell + size.x].visited)
            neighbors.Add(cell + size.x);

        // Right
        if ((cell + 1) % size.x != 0 && !board[cell + 1].visited)
            neighbors.Add(cell + 1);

        // Left
        if (cell % size.x != 0 && !board[cell - 1].visited)
            neighbors.Add(cell - 1);

        return neighbors;
    }

    void SpawnEnemies(Vector3Int roomOrigin, RoomData roomData)
    {
        if (enemyPrefab == null) return;

        int enemyCount = Random.Range(minEnemiesPerRoom, maxEnemiesPerRoom + 1);

        float padding = 2.0f; // 离墙安全距离

        for (int i = 0; i < enemyCount; i++)
        {
            // 在房间内随机位置生成敌人
            Vector3 spawnPos = roomOrigin + new Vector3(
                Random.Range(1, roomSize - padding),
                -Random.Range(1, roomSize - padding),
                0);

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, enemyContainer);
            enemy.SetActive(false); // 初始未激活
            roomData.enemies.Add(enemy);
        }
    }


    void CloseDoors(Vector3Int roomPos)
    {
        var room = roomDataDict[roomPos];
        foreach (var doorPos in room.doorPositions)
        {
            doorTilemap.SetTile(doorPos, wallTile);
        }
    }

    void OpenDoors(Vector3Int roomPos)
    {
        var room = roomDataDict[roomPos];
        foreach (var doorPos in room.doorPositions)
        {
            doorTilemap.SetTile(doorPos, doorTile);
        }
    }
    void OnPlayerEnterNewRoom(Vector3Int roomPos)
    {
        if (!roomDataDict.ContainsKey(roomPos)) return;

        var room = roomDataDict[roomPos];

        if (!room.isActive && room.enemies.Count > 0)
        {
            room.isActive = true;
            CloseDoors(roomPos); // 改为关闭三格门

            foreach (var enemy in room.enemies)
            {
                enemy.SetActive(true);
            }
        }
    }

    void CheckRoomCleared(Vector3Int roomPos)
    {
        var room = roomDataDict[roomPos];

        bool allDead = true;
        foreach (var enemy in room.enemies)
        {
            if (enemy != null)
            {
                allDead = false;
                break;
            }
        }

        if (allDead)
        {
            room.isActive = false;
            OpenDoors(roomPos); // 改为打开三格门
        }
    }

    void Update()
    {
        if (roomDataDict.Count == 0) return;

        // 检测玩家当前所在房间
        Vector3Int playerRoomPos = GetCurrentRoom(PlayerPosition());

        if (playerRoomPos != currentPlayerRoom)
        {
            OnPlayerEnterNewRoom(playerRoomPos);
            currentPlayerRoom = playerRoomPos;
        }

        // 检查当前房间敌人是否全部被消灭
        if (roomDataDict.ContainsKey(currentPlayerRoom) &&
            roomDataDict[currentPlayerRoom].isActive)
        {
            CheckRoomCleared(currentPlayerRoom);
        }
    }

    Vector3 PlayerPosition()
    {
        return GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    Vector3Int GetCurrentRoom(Vector3 position)
    {
        int roomX = Mathf.FloorToInt(position.x / (roomSize + 1));
        int roomY = Mathf.FloorToInt(-position.y / (roomSize + 1));
        return new Vector3Int(roomX * (roomSize + 1), -roomY * (roomSize + 1), 0);
    }
}
