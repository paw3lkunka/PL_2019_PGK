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

    private void OnValidate()
    {
        grid = GetComponent<Grid>();
        if( !ValidatePrefabs() )
        {
            Debug.LogError("Location prefabs validation failed!");
        }
    }

    [ContextMenu("Validate")]
    public bool ValidatePrefabs()
    {
        foreach (GameObject prefab in locationPrefabs)
        {
            int scriptsCount = prefab.GetComponentsInChildren<Location>().Length;

            if (scriptsCount != 1)
            {
                throw new LocationValidationException(prefab, scriptsCount);
            }
        }
        Debug.Log("Location prefabs validated");
        return true;
    }

    [ContextMenu("Generate")]
    private void Generate()
    {
        if(!useCustomSeed)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }

        Random.InitState(seed);

        List<int> chances = new List<int>();
        int range = 0;

        foreach (GameObject LocationObject in locationPrefabs)
        {
            int chance = LocationObject.GetComponent<Location>().SpawnChance;
            range += chance;
            chances.Add(chance);
        }

        for (int i = 0; i < segments.x; i++)
        {
            for (int j = 0; j < segments.y; j++)
            {
                int randomNumber = Random.Range(0, range);
                int index = 0;

                Vector3 position = new Vector3(i * cellSize.x, j * cellSize.y);
                position.x += Random.Range(-randomOffsetRange.x, randomOffsetRange.x);
                position.y += Random.Range(-randomOffsetRange.y, randomOffsetRange.y);
                
                while (chances[index] < randomNumber)
                {
                    randomNumber -= chances[index];
                    index++;
                }
                
                GameObject instance = Instantiate(locationPrefabs[index], position, Quaternion.identity, grid.transform);
                DestroyImmediate(instance.GetComponent<Grid>());
            }
        }
    }

    [ContextMenu("Clear")]
    private void Clear()
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