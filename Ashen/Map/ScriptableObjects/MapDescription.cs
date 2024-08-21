using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDescription : SerializedScriptableObject
{
    [TableMatrix]
    public int[,] map;

    public Vector2Int defaultPosition;
    public RotationDirection defaultRotation;

    public int openFloorValue;
    public int wallValue;
    public int startingValue;

    public GameObject wall;
    public GameObject floor;
    public GameObject ceiling;

    public bool RequestMove(Vector2Int currentPos, Vector2Int targetPos)
    {
        int x = targetPos.x;
        int z = targetPos.y;
        if (x < 0 || z < 0 || x >= map.GetLength(0) || z >= map.GetLength(1))
        {
            return false;
        }
        return map[x, z] == openFloorValue || map[x, z] == startingValue;
    }

    public float GetEncounterMultiplier(Vector2Int position)
    {
        return 1f;
    }
}
