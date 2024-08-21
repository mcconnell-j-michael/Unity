using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class MapBuilder : SingletonMonoBehaviour<MapBuilder>
{
    private GameObject floor;
    private GameObject ceiling;
    private List<GameObject> walls;

    public MapDescription mapDescription;
    public GameObject levelHolder;

    [Button]
    public void BuildLevel()
    {
        if (!levelHolder)
        {
            levelHolder = new GameObject();
        }
        levelHolder.name = "LevelHolder";
        ResetLevel();

        if (walls == null)
        {
            walls = new List<GameObject>();
        }

        int[,] map = mapDescription.map;

        int width = map.GetLength(1);
        int height = map.GetLength(0);

        float floorX = (width - 1) / 2f;
        float floorZ = (height - 1) / 2f;

        float floorXScale = width;
        float floorZScale = height;

        floor = Instantiate(mapDescription.floor, levelHolder.transform);
        floor.transform.localScale = new Vector3(floorXScale, floorZScale, floor.transform.localScale.z);
        floor.transform.position = new Vector3(floorX, floor.transform.position.y, floorZ);

        ceiling = Instantiate(mapDescription.ceiling, levelHolder.transform);
        ceiling.transform.localScale = new Vector3(floorXScale, floorZScale, floor.transform.localScale.z);
        ceiling.transform.position = new Vector3(floorX, ceiling.transform.position.y, floorZ);

        int z = 0;

        for (int idz = height - 1; idz >= 0; idz--)
        {
            int x = 0;
            for (int idx = 0; idx < width; idx++)
            {
                
                if (map[x, z] == mapDescription.wallValue)
                {
                    GameObject wall = Instantiate(mapDescription.wall, levelHolder.transform);
                    walls.Add(wall);
                    wall.transform.position = new Vector3(x, wall.transform.position.y, z);
                }
                x++;
            }
            z++;
        }
    }

    private void ResetLevel()
    {
        if (floor)
        {
            DestroyImmediate(floor);
            floor = null;
        }
        if (ceiling)
        {
            DestroyImmediate(ceiling);
            ceiling = null;
        }
        if (walls != null)
        {
            for (int x = 0; x < walls.Count; x++)
            {
                DestroyImmediate(walls[x]);
            }
            walls.Clear();
        }
    }
}
