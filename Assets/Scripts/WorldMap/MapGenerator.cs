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

    /// <summary>
    /// Determine, if Generate function should randomize new seed.
    /// </summary>
    public bool useCustomSeed = false;

    /// <summary>
    /// Guarantee, that cente of map will be empty
    /// </summary>
    public bool forceEmptyCentre = false;

    private bool templeGenerated = false;

    public int seed;

    [SerializeField] public GameObject shrineLocationPrefab;
    [SerializeField] public GameObject templeLocationPrefab;
    
    /// <summary>
    /// Size of grid cells
    /// </summary>
    public Vector2Int cells = new Vector2Int(5, 5);

    /// <summary>
    /// Size of single cell
    /// </summary>
    public Vector2Int cellSize = new Vector2Int(30, 30);

    /// <summary>
    /// Range off possible distance from cell centre to generated object
    /// </summary>
    public Vector2Int randomOffsetRange = new Vector2Int(0, 0);

    /// <summary>
    /// Chance of creating empty cell
    /// </summary>
    [HideInInspector] public int emptyChance;

    /// <summary>
    /// Inform if all prefabs in list are valid
    /// </summary>
    public bool IsValid { get; private set; }

    /// <summary>
    /// Chances of spawning objecton specific index in prefab array
    /// </summary>
    [HideInInspector] public List<int> spawnChances = new List<int>();


    /// <summary>
    /// Coordinates in empty cells, possible locations of temple;
    /// </summary>
    private List<Vector3> unusedPositions = new List<Vector3>();

    /// <summary>
    /// Gets location prefabs from prefab database without Application Manager
    /// </summary>
    public List<GameObject> Locations { get; private set; }

    #endregion

    #region MonoBehaviour

    private void OnValidate() => Initialize();

    private void Awake() => Initialize();

    private void LateUpdate()
    {
        if( !templeGenerated && GameplayManager.Instance.ShrinesVisited.Count == 3)
        {
            int randomPos = Random.Range(0, unusedPositions.Count);
            GameObject instance = Instantiate(templeLocationPrefab, unusedPositions[randomPos], Quaternion.identity, transform);
            templeGenerated = true;
        }
    }

    #endregion

    #region Component

    private void Initialize()
    {
        Locations = (Resources.Load("PrefabDatabase") as PrefabDatabase).locations;

        spawnChances.Resize(Locations.Count, 0);

        ValidatePrefabs();

        if (cells.x < 1)
            cells.x = 1;
        if (cells.y < 1)
            cells.y = 1;

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
        foreach (GameObject prefab in Locations)
        {
            int scriptsCount = prefab.GetComponentsInChildren<Location>().Length;

            if (scriptsCount != 1)
            {
                IsValid = false;
                throw new LocationValidationException(prefab, scriptsCount);
            }
        }
        IsValid = true;
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

        for (int i = 0; i < Locations.Count; i++)
        {
            int chance = spawnChances[i];
            range += chance;
            chances.Add(chance);
        }

        range += emptyChance;
        chances.Add(emptyChance);

        int halfWidth = (cells.x - 1) * cellSize.x / 2;
        int halfHight = (cells.y - 1) * cellSize.y / 2;

        for (int i = 0; i < cells.x; i++)
        {
            for (int j = 0; j < cells.y; j++)
            {
                int randomNumber = Random.Range(0, range);
                int index = 0;

                while (index < Locations.Count && chances[index] < randomNumber)
                {
                    randomNumber -= chances[index];
                    index++;
                }
                float yCoord = index < Locations.Count ? Locations[index].transform.position.y : 0;
                Vector3 position = new Vector3(i * cellSize.x, yCoord, j * cellSize.y);
                position.x += Random.Range(-randomOffsetRange.x, randomOffsetRange.x) - halfWidth;
                position.z += Random.Range(-randomOffsetRange.y, randomOffsetRange.y) - halfHight;
                unusedPositions.Add(position);

                if (index < Locations.Count)
                {
                    Instantiate(Locations[index], position, Quaternion.identity, transform);
                    unusedPositions.RemoveAt(unusedPositions.Count - 1);
                }

                if (forceEmptyCentre)
                {
                    FreeCentre();
                }
            }
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        foreach (var child in GetComponentsInChildren<Location>())
        {
            DestroyImmediate(child.gameObject);
        }
    }

    private void FreeCentre()
    {
        foreach (Location loc in transform.GetComponentsInChildren<Location>())
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
