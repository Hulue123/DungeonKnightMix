using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class DungeonTileRule
{
    public TileBase tile;
    public Vector2Int minPosition;
    public Vector2Int maxPosition;
    public bool obligatory;

    public int ProbabilityOfSpawning(int x, int y)
    {
        if (x >= minPosition.x && x <= maxPosition.x &&
            y >= minPosition.y && y <= maxPosition.y)
        {
            return obligatory ? 2 : 1;
        }
        return 0;
    }
}