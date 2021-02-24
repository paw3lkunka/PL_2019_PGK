using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    public const int ZONES = 3;
    public int spawnRadius = 2;
    public int seed;

    public float locationsScale = 1;

    public Grid grid;

    private static bool gridGeneraed = false;
    private static List<GameObject>[,] lookupPrefabs;
    private static List<Vector3>[,] lookupPositions;
    private static List<float>[,] lookupRotations;

    private int lastCellHash;
    [NonReorderable]
    public List<Zone> prefabsPerZone;


    private void Start()
    {
    }


    private void Update()
    {
        var currCellIndex = grid.GetNear(WorldSceneManager.Instance.Leader.transform.position);
        var currCellHash = currCellIndex.GetHashCode();

        if (lastCellHash != currCellHash)
        {
            lastCellHash = currCellHash;
            SpawnCell(currCellIndex.x, currCellIndex.y);
        }
    }

    public void Generate(Vector3 position)
    {
        grid.Generate(transform, transform.position);

        if (!gridGeneraed)
        {
            GenerateOffline();
            gridGeneraed = true;
        }

        int X = grid.cellsNumber.x;
        int Y = grid.cellsNumber.y;
    }

    private void SpawnCell(int x, int y)
    {
        if (!grid.Cells[x,y].spawned)
        {
            for (int i = 0; i < lookupPrefabs[x, y].Count; i++)
            {
                var obj = Instantiate(lookupPrefabs[x,y][i], lookupPositions[x,y][i], Quaternion.AngleAxis(lookupRotations[x,y][i], Vector3.up), grid.Cells[x, y].transform);
                obj.transform.localScale *= locationsScale;
            }

            grid.Cells[x, y].spawned = true;
        }

    }

    private void DespawnCell(int x, int y)
    {
        if (grid.Cells[x, y].spawned)
        {
            var transform = grid.Cells[x, y].transform;
            
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            grid.Cells[x, y].spawned = false;
        }
    }

    private void GenerateOffline()
    {
        int X = grid.cellsNumber.x;
        int Y = grid.cellsNumber.y;

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
                lookupPrefabs[i, j].Add(GetRandom(prefabsPerZone[cell.zone].locations));
                lookupPositions[i, j].Add(grid.Cells[i, j].transform.position);
                lookupRotations[i, j].Add(Random.Range(0.0f, 360.0f));

                if (cell.zone == 0)
                {
                    Debug.Log(lookupPrefabs[i, j].Last());
                }

            }
        }
    }

    private void GeneriteRuntime()
    {

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

    #region old

    private bool IntersectionTest(GameObject envObject, IEnumerable<GameObject> locInstances)
    {
        foreach (GameObject instance in locInstances)
        {
            var colliders = envObject.GetComponentsInChildren<Collider>();

            foreach (var collider in colliders)
            {
                if (instance.GetComponent<Collider>().bounds.Intersects(collider.bounds))
                {
                    return false;
                }
            }
        }
        return true;
    }

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

