using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public const int ZONES = 3;
    public int seed;

    public GameObject PLACEHOLDER;

    public Grid grid;

    private static bool gridGeneraed = false;
    private static List<GameObject>[,] lookupPrefabs;
    private static List<Vector3>[,] lookupPositions;

    private void Start()
    {
    }

    public void Generate(Vector3 position)
    {
        grid.Generate(transform, transform.position);

        if (!gridGeneraed)
        {
            GenerateOffline();
            gridGeneraed = true;
        }

        for (int i = 0; i < grid.cellsNumber.x; i+=2)
        {
            for (int j = 0; j < grid.cellsNumber.y; j+=2)
            {
                SpawnCell(i, j);
            }
        }
    }

    private void SpawnCell(int x, int y)
    {
        for (int i = 0; i < lookupPrefabs[x, y].Count; i++)
        {
            var obj = Instantiate(lookupPrefabs[x,y][i], lookupPositions[x,y][i], Quaternion.identity);
            obj.transform.parent = grid.Cells[x, y].transform;
        }
    }

    private void DespawnCell(int x, int y)
    {

    }

    private void GenerateOffline()
    {
        int X = grid.cellsNumber.x;
        int Y = grid.cellsNumber.y;

        lookupPrefabs = new List<GameObject>[X, Y];
        lookupPositions = new List<Vector3>[X, Y];

        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                lookupPrefabs[i, j] = new List<GameObject>();
                lookupPositions[i, j] = new List<Vector3>();

                lookupPrefabs[i, j].Add(PLACEHOLDER);
                lookupPositions[i, j].Add(grid.Cells[i, j].transform.position);
            }
        }
    }

    private void GeneriteRuntime()
    {

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

