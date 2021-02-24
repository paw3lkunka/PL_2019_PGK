using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public const int ZONES = 3;
    public int spawnRadius = 2;
    public int seed;

    public GameObject PLACEHOLDER;

    public float locationsScale = 1;

    public Grid grid;

    private static bool gridGeneraed = false;
    private static List<GameObject>[,] lookupPrefabs;
    private static List<Vector3>[,] lookupPositions;
    private static List<float>[,] lookupRotations;

    private int lastCellHash;

    private void Start()
    {
    }


    private void Update()
    {

        var currCellIndex = grid.GetNear(WorldSceneManager.Instance.Leader.transform.position);
        var currCell = grid.Cells[currCellIndex.x, currCellIndex.y];
        var currCellHash = currCell.name.GetHashCode();

        Debug.LogWarning(currCellIndex);
        if (lastCellHash != currCellHash)
        {
            Debug.Log($"{lastCellHash} =/= {currCellHash}");
            lastCellHash = currCellHash;
            SpawnCell(currCellIndex.x, currCellIndex.y);
        }
        else
        {
            Debug.Log($"{lastCellHash} == {currCellHash}");
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

        //for (int i = 0; i < X; i++)
        //{
        //    for (int j = 0; j < Y; j++)
        //    {
        //        SpawnCell(i, j);
        //    }
        //}
    }

    private void SpawnCell(int x, int y)
    {
        for (int i = 0; i < lookupPrefabs[x, y].Count; i++)
        {
            var obj = Instantiate(lookupPrefabs[x,y][i], lookupPositions[x,y][i], Quaternion.AngleAxis(lookupRotations[x,y][i], Vector3.up), grid.Cells[x, y].transform);
            obj.transform.localScale *= locationsScale;
        }
    }

    private void DespawnCell(int x, int y)
    {
        var transform = grid.Cells[x, y].transform;
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void GenerateOffline()
    {
        int X = grid.cellsNumber.x;
        int Y = grid.cellsNumber.y;

        lookupPrefabs = new List<GameObject>[X, Y];
        lookupPositions = new List<Vector3>[X, Y];
        lookupRotations = new List<float>[X, Y];

        for (int i = 0; i < X; i += 1)
        {
            //   PPPP   L       AAA    CCCC  EEEEE  H   H    OOO   L      DDDD   EEEEE  RRRR             
            //   P   P  L      A   A  C      E      H   H   O   O  L      D   D  E      R   R            
            //   P   P  L      A   A  C      E      H   H   O   O  L      D   D  E      R   R            
            //   PPPP   L      AAAAA  C      EEE    HHHHH   O   O  L      D   D  EEE    RRRR             
            //   P      L      A   A  C      E      H   H   O   O  L      D   D  E      R   R            
            //   P      LLLLL  A   A   CCCC  EEEEE  H   H    OOO   LLLLL  DDDD   EEEEE  R   R            
            
            for (int j = 0; j < Y; j += 1)
            {
                lookupPrefabs[i, j] = new List<GameObject>();
                lookupPositions[i, j] = new List<Vector3>();
                lookupRotations[i, j] = new List<float>();

                lookupPrefabs[i, j].Add(PLACEHOLDER);
                lookupPositions[i, j].Add(grid.Cells[i, j].transform.position);
                lookupRotations[i, j].Add(Random.Range(0.0f, 360.0f));
            }
        }
    }

    private void GeneriteRuntime()
    {

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

