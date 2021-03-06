using System.Collections.Generic;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    public const int ZONES = 3;
    public int spawnRadius = 2;
    public int seed;

    public float locationsScale = 1;
    public float obstaclesScale = 1;

    public Grid grid;

    public List<Zone> prefabsPerZone;
    
    private static bool gridGeneraed = false;
    private static List<GameObject>[,] lookupPrefabs;
    private static List<Vector3>[,] lookupPositions;
    private static List<float>[,] lookupRotations;

    private int lastCellHash;

    private void Update()
    {
        var currCellIndex = grid.GetNear(WorldSceneManager.Instance.Leader.transform.position);
        var currCellHash = currCellIndex.GetHashCode();

        if (lastCellHash != currCellHash)
        {
            lastCellHash = currCellHash;

            for (int i = 0; i < grid.cellsInRow; i++)
            {
                for (int j = 0; j < grid.cellsInRow; j++)
                {
                    if ( i >= currCellIndex.x - spawnRadius
                      && i <= currCellIndex.x + spawnRadius
                      && j >= currCellIndex.y - spawnRadius
                      && j <= currCellIndex.y + spawnRadius)
                    {
                        SpawnCell(i, j);
                    }
                    else
                    {
                        DespawnCell(i, j);
                    }
                }
            }
        }
    }

    public void Generate()
    {
        grid.Generate(transform, transform.position);

        if (!gridGeneraed)
        {
            GenerateOffline();
            gridGeneraed = true;
        }
    }

    private bool SpawnCell(int x, int y)
    {
        if (!grid.Cells[x,y].spawned)
        {
            var location = Instantiate(lookupPrefabs[x,y][0], lookupPositions[x,y][0], Quaternion.AngleAxis(lookupRotations[x,y][0], Vector3.up), grid.Cells[x,y].transform);
            location.transform.localScale *= locationsScale;

            for (int i = 1; i < lookupPrefabs[x, y].Count; i++)
            {
                var obstacle = Instantiate(lookupPrefabs[x,y][i], lookupPositions[x,y][i], Quaternion.AngleAxis(lookupRotations[x,y][i], Vector3.up), grid.Cells[x,y].transform);
                obstacle.transform.localScale *= obstaclesScale;

                if (IntersectionTest(location, obstacle))
                {
                    Debug.Log("Intersection");
                    Destroy(obstacle);
                    lookupPrefabs[x, y].RemoveAt(i);
                    lookupPositions[x, y].RemoveAt(i);
                    lookupRotations[x, y].RemoveAt(i);
                    i--;
                }
            }

            grid.Cells[x, y].spawned = true;
            return true;
        }

        return false;
    }

    private void DespawnCell(int x, int y)
    {
        var transform = grid.Cells[x, y].transform;

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        grid.Cells[x, y].spawned = false;
    }

#if UNITY_EDITOR
    public void Preview()
    {
        if (Application.isEditor)
        {
            Generate();

            for (int i = 0; i < grid.cellsInRow; i++)
            {
                for (int j = 0; j < grid.cellsInRow; j++)
                {
                    SpawnCell(i, j);
                }
            }
        }
    }

    public void ClearPreview()
    {
        if (Application.isEditor)
        {
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }

            gridGeneraed = false;
        }
    }
#endif

    private void GenerateOffline()
    {
        Random.InitState(seed);

        int X = grid.cellsInRow;
        int Y = grid.cellsInRow;

        lookupPrefabs = new List<GameObject>[X, Y];
        lookupPositions = new List<Vector3>[X, Y];
        lookupRotations = new List<float>[X, Y];

        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                lookupPrefabs[i, j] = new List<GameObject>();
                lookupPositions[i, j] = new List<Vector3>();
                lookupRotations[i, j] = new List<float>();

                var cell = grid.Cells[i, j];
                var prefab = GetRandom(prefabsPerZone[cell.zone].locations);
                var position = grid.Cells[i, j].transform.position;
                var offset = new Vector3(Random.value - 0.5f, 0, Random.value - 0.5f) * grid.cellSize;

                lookupPrefabs[i, j].Add(prefab);
                lookupPositions[i, j].Add(position + offset);
                lookupRotations[i, j].Add(Random.Range(0.0f, 360.0f));

                int zone = cell.zone;
                int obstacles = Random.Range(prefabsPerZone[zone].minObsacles, prefabsPerZone[zone].maxObstacles + 1);

                for (int k = 0; k < obstacles; k++)
                {
                    prefab = GetRandom(prefabsPerZone[cell.zone].obstacles);
                    position = grid.Cells[i, j].transform.position;
                    offset = new Vector3(Random.value - 0.5f, 0, Random.value - 0.5f) * grid.cellSize;

                    lookupPrefabs[i, j].Add(prefab);
                    lookupPositions[i, j].Add(position + offset);
                    lookupRotations[i, j].Add(Random.Range(0.0f, 360.0f));
                }
            }
        }
    }

    static public GameObject GetRandom(List<PrefabWrapper> pairs)
    {
        float range = 0;

        foreach (var pair in pairs)
        {
            range += pair.spawnChance;
        }

        float randomNumber = Random.Range(0, range);
        int index = 0;

        while (randomNumber > 0)
        {
            randomNumber -= pairs[index++].spawnChance;  
        }

        return pairs[--index].prefab;
    }

    private bool IntersectionTest(GameObject location, GameObject obstacle)
    {
        var collider1 = location.GetComponent<Collider>();
        var colliders = obstacle.GetComponentsInChildren<Collider>();

        foreach (var collider2 in colliders)
        {
            if (collider1.bounds.Intersects(collider2.bounds))
            {
                return true;
            }
        }
        return false;
    }

    #region old

    public void SaveState()
    {
        foreach (var loc in GetComponentsInChildren<Location>())
        {
            loc.SaveState();
        }
        PlayerPrefs.Save();
    }

    public void LoadState()
    {
        foreach (var loc in GetComponentsInChildren<Location>())
        {
            loc.LoadState();
        }
    }

    public void ClearSave()
    {
        foreach (var loc in GetComponentsInChildren<Location>())
        {
            loc.ClearSave();
        }
        PlayerPrefs.Save();
    }

    #endregion
}

