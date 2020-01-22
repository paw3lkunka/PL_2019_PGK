using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class LocationValidationException : System.Exception
{

    public LocationValidationException(GameObject obj, int number)
        : base( number == 0 ? obj + " is not a location" : obj + " has multiple Location scripts (" + number + ")")
    { }
}

public class MapGenerator : MonoBehaviour
{
    public bool useCustomSeed = false;
    public int seed;

    public List<GameObject> locationPrefabs;

    private Grid grid;

    public Vector2Int segments = new Vector2Int(5, 5);
    public Vector2Int cellSize = new Vector2Int(30, 30);
    public Vector2Int randomOffsetRange = new Vector2Int(0, 0);

    [HideInInspector] public int emptyChance;
    [HideInInspector] public bool isValid;

    private void OnValidate()
    {
        grid = GetComponent<Grid>();
        ValidatePrefabs();

        if (segments.x < 1)
            segments.x = 1;
        if (segments.y < 1)
            segments.y = 1;

        if (cellSize.x < 1)
            cellSize.x = 1;
        if (cellSize.y < 1)
            cellSize.y = 1;

        if (randomOffsetRange.x < 0)
            randomOffsetRange.x = 0;
        if (randomOffsetRange.y < 0)
            randomOffsetRange.y = 0;
    }

    [ContextMenu("Validate")]
    public void ValidatePrefabs()
    {
        foreach (GameObject prefab in locationPrefabs)
        {
            int scriptsCount = prefab.GetComponentsInChildren<Location>().Length;

            if (scriptsCount != 1)
            {
                isValid = false;
                throw new LocationValidationException(prefab, scriptsCount);
            }
        }
        isValid = true;
    }

    [ContextMenu("Generate")]
    public void Generate()
    {
        if(!useCustomSeed)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }

        Random.InitState(seed);

        List<int> chances = new List<int>() ;
        int range = 0;

        foreach (GameObject LocationObject in locationPrefabs)
        {
            int chance = LocationObject.GetComponent<Location>().spawnChance;
            range += chance;
            chances.Add(chance);
        }

        range += emptyChance;
        chances.Add(emptyChance);

        int halfWidth = (segments.x-1) * cellSize.x / 2;
        int halfHight = (segments.y-1) * cellSize.y / 2;

        for (int i = 0; i < segments.x; i++)
        {
            for (int j = 0; j < segments.y; j++)
            {
                int randomNumber = Random.Range(0, range);
                int index = 0;

                Vector3 position = new Vector3(i * cellSize.x, j * cellSize.y);
                position.x += Random.Range(-randomOffsetRange.x, randomOffsetRange.x) - halfWidth;
                position.y += Random.Range(-randomOffsetRange.y, randomOffsetRange.y) - halfHight;
                
                while (chances[index] < randomNumber)
                {
                    randomNumber -= chances[index];
                    index++;
                }
                
                try
                {
                    GameObject instance = Instantiate(locationPrefabs[index], position, Quaternion.identity, grid.transform);
                    DestroyImmediate(instance.GetComponent<Grid>());
                }
                catch( System.ArgumentOutOfRangeException exc )
                {
                    //if index == locationPrefabs.Count cell should be empty - it's normal situation, else rethrow
                    if (index != locationPrefabs.Count)
                        throw exc;
                }
            }
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        foreach( Transform child in grid.GetComponentsInChildren<Transform>())
        {
            if(child.name.Contains("(Clone)"))
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}


/*
[RequireComponent(typeof(Grid))]
public class MapGenerator : MonoBehaviour
{
    private Grid grid;

    Vector2Int segments = new Vector2Int(5, 5);
    Vector2Int cellSize = new Vector2Int(30, 30);
    Vector2Int randomOffsetRange = new Vector2Int(0, 0);

    public GameObject LocationsGridPrefab;

    private void Awake()
    {
    }

    private void Start()
    {

    }

    [ContextMenu("Generate")]
    private void Generate()
    {
        List<int> chances = new List<int>();
        int range = 0;

        foreach( Location location in LocationsGridPrefab.GetComponentsInChildren<Location>() )
        {
            int chance = location.SpawnChance;
            range += chance;
            chances.Add(chance);
        }

        for (int i = 0; i < segments.x; i++)
        {
            for (int j = 0; j < segments.y; j++)
            {
                int randomNumber = Random.Range(0, range);
                int index = 0;

                Vector3 position = new Vector3(i*cellSize.x, j*cellSize.y);
                
                Debug.Log(chances.Count);

                while ( chances[index] < randomNumber)
                {
                    randomNumber -= chances[index];
                    index++;
                }

                Debug.Log("index: " + index);

                Instantiate(LocationsGridPrefab.transform.GetChild(index).gameObject, position, Quaternion.identity, grid.transform);
            }
        }
    }


}
*/