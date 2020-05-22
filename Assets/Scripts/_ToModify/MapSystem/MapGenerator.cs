using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

class LocationValidationException : System.Exception
{
    public LocationValidationException(GameObject obj, int number)
        : base( number == 0 ? obj + " is not a location" : obj + " has multiple Location scripts (" + number + ")")
    { 
        
    }
}

public class MapGenerator : MonoBehaviour
{
    #region Variables

    public bool useCustomSeed = false;
    public bool forceEmptyCentre = false;
    public bool foregroundMap = false;

    private bool templeGenerated = false;

    public int seed;
    public int orderInLayer;

    [SerializeField] public GameObject shrineLocationPrefab;
    [SerializeField] public GameObject templeLocationPrefab;
    public List<GameObject> locationPrefabs = new List<GameObject>();
    

    private Grid grid;

    public Vector2Int segments = new Vector2Int(5, 5);
    public Vector2Int cellSize = new Vector2Int(30, 30);
    public Vector2Int randomOffsetRange = new Vector2Int(0, 0);

    [HideInInspector] public int emptyChance;
    [HideInInspector] public bool isValid;
    [HideInInspector] public List<int> spawnChances = new List<int>();


    public int generationID;

    private List<Vector3> positions = new List<Vector3>();
    private List<Vector3> unusedPositions = new List<Vector3>();

    #endregion

    #region MonoBehaviour

    private void OnValidate()
    {
        Initialize();
    }

    private void Awake()
    {
        Initialize();
    }

    private void LateUpdate()
    {
        if(foregroundMap && !templeGenerated && GameplayManager.Instance.ShrinesVisited.Count == 3)
        {
            int randomPos = Random.Range(0, unusedPositions.Count);
            GameObject instance = Instantiate(templeLocationPrefab, unusedPositions[randomPos], Quaternion.identity, grid.transform);
            instance.GetComponentInChildren<Location>().generationID = (int)generationID;
            instance.GetComponentInChildren<TilemapRenderer>().sortingOrder = orderInLayer;
            templeGenerated = true;
        }
    }

    #endregion

    #region Component

    private void Initialize()
    {

        grid = GetComponent<Grid>();
        spawnChances.Resize(locationPrefabs.Count, 0);

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

        if (!useCustomSeed)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }

        Random.InitState(seed);

        List<int> chances = new List<int>();
        int range = 0;

        for (int i = 0; i < locationPrefabs.Count; i++)
        {
            int chance = spawnChances[i];
            range += chance;
            chances.Add(chance);
        }

        range += emptyChance;
        chances.Add(emptyChance);

        int halfWidth = (segments.x - 1) * cellSize.x / 2;
        int halfHight = (segments.y - 1) * cellSize.y / 2;

        for (int i = 0; i < segments.x; i++)
        {
            for (int j = 0; j < segments.y; j++)
            {
                int randomNumber = Random.Range(0, range);
                int index = 0;

                Vector3 position = new Vector3(i * cellSize.x, 0.0f, j * cellSize.y);
                position.x += Random.Range(-randomOffsetRange.x, randomOffsetRange.x) - halfWidth;
                position.z += Random.Range(-randomOffsetRange.y, randomOffsetRange.y) - halfHight;
                unusedPositions.Add(position);

                while (index < locationPrefabs.Count && chances[index] < randomNumber)
                {
                    randomNumber -= chances[index];
                    index++;
                }

                if(index < locationPrefabs.Count)
                {
                    GameObject instance = Instantiate(locationPrefabs[index], position, Quaternion.identity, grid.transform);
                    positions.Add(position);
                    unusedPositions.RemoveAt(unusedPositions.Count - 1);
                    instance.GetComponentInChildren<Location>().generationID = (int)generationID;
                    instance.GetComponentInChildren<TilemapRenderer>().sortingOrder = orderInLayer;
                }

                if (forceEmptyCentre)
                {
                    FreeCentre();
                }
            }
        }
        if (foregroundMap)
        {
            for (int i = 0; i < 3; ++i)
            {
                int randomIndex;
                while (true)
                {
                    randomIndex = Random.Range(0, positions.Count);
                    if ((positions[randomIndex].x > 20 || positions[randomIndex].x < -20) && (positions[randomIndex].z > 20 || positions[randomIndex].z < -20))
                    {
                        for (int j = 0; j < transform.childCount; ++j)
                        {
                            if (transform.GetChild(j).position == positions[randomIndex])
                            {
                                #if UNITY_EDITOR
                                DestroyImmediate(transform.GetChild(j).gameObject);
                                #else
                                Destroy(transform.GetChild(j).gameObject);
                                #endif
                                GameObject instance = Instantiate(shrineLocationPrefab, positions[randomIndex], Quaternion.identity, grid.transform);
                                instance.GetComponentInChildren<Location>().generationID = (int)generationID;
                                instance.GetComponentInChildren<TilemapRenderer>().sortingOrder = orderInLayer;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        foreach (var child in grid.GetComponentsInChildren<Location>())
        {
            if (child.generationID == generationID)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        generationID = GetHashCode();
    }

    private void FreeCentre()
    {
        foreach (Location loc in transform.GetComponentsInChildren<Location>())
        {
            if (loc.generationID == generationID)
            {
                Vector2 halfSize = (Vector2)cellSize * 0.5f;
                Vector3 pos = loc.transform.localPosition;

                if (pos.x < halfSize.x && pos.x > -halfSize.x && pos.z < halfSize.y && pos.z > -halfSize.y)
                {
                    pos.x = pos.x > 0 ? halfSize.x : -halfSize.x;
                    pos.z = pos.z > 0 ? halfSize.y : -halfSize.y;
                }

                loc.transform.localPosition = pos;
            }
        }

    }

#endregion
}

public static class ListExtra
{
    public static void Resize<T>(this List<T> list, int sz, T c)
    {
        int cur = list.Count;
        if (sz < cur)
            list.RemoveRange(sz, cur - sz);
        else if (sz > cur)
        {
            if (sz > list.Capacity)//this bit is purely an optimisation, to avoid multiple automatic capacity changes.
                list.Capacity = sz;
            list.AddRange(System.Linq.Enumerable.Repeat(c, sz - cur));
        }
    }
    public static void Resize<T>(this List<T> list, int sz) where T : new()
    {
        Resize(list, sz, new T());
    }
}
