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
    public TileBase defaultGroundTile;

    [Header("Dungeon Settings")]
    public Vector2Int size = new Vector2Int(5, 5); // 房间数量
    public int roomSize = 3; // 每个房间的大小(3x3)
    public int startPos = 0;

    private List<Cell> board;

    private class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4]; // 0-Up, 1-Down, 2-Right, 3-Left
    }

    void Start()
    {
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
                    // 填充房间地板
                    FillRoomWithFloor(roomOrigin);

                    // 生成墙壁和门
                    GenerateWallsAndDoors(roomOrigin, currentCell);
                }
                else
                {
                    // 填充未访问房间为墙
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
        for (int x = 0; x <= roomSize ; x++)
        {
            Vector3Int wallPos = origin + new Vector3Int(x, 1, 0);
            if (cell.status[0]) { // 有上门
                if (x == 6 || x == 7 || x == 8)
                    doorTilemap.SetTile(wallPos, doorTile);
                else
                    wallTilemap.SetTile(wallPos, wallTile);
            }
            else
                wallTilemap.SetTile(wallPos, wallTile);
        }

        // 下墙壁/门
        for (int x = 0; x <= roomSize ; x++)
        {
            Vector3Int wallPos = origin + new Vector3Int(x, -roomSize, 0);
            if (cell.status[1]) // 有下门
                if (x == 6 || x == 7 || x == 8)
                    doorTilemap.SetTile(wallPos, doorTile);
                else
                    wallTilemap.SetTile(wallPos, wallTile);
            else
                wallTilemap.SetTile(wallPos, wallTile);
        }

        // 右墙壁/门
        for (int y = 0; y <= roomSize ; y++)
        {
            Vector3Int wallPos = origin + new Vector3Int(roomSize, -y, 0);
            if (cell.status[2]) // 有右门
                if (y == 6 || y == 7 || y == 8)
                    doorTilemap.SetTile(wallPos, doorTile);
                else
                    wallTilemap.SetTile(wallPos, wallTile);
            else
                wallTilemap.SetTile(wallPos, wallTile);
        }

        // 左墙壁/门
        for (int y = 0; y <= roomSize ; y++)
        {
            Vector3Int wallPos = origin + new Vector3Int(-1, -y, 0);
            if (cell.status[3]) // 有左门
                if (y == 6 || y == 7 || y == 8)
                    doorTilemap.SetTile(wallPos, doorTile);
                else
                    wallTilemap.SetTile(wallPos, wallTile);
            else
                wallTilemap.SetTile(wallPos, wallTile);
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
}
