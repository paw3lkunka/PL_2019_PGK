using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{   
    public const int ZONES = 3;

    [field: SerializeField] public Grid GeneralGrid { get; private set; }

    #region Variables

    /// <summary>
    /// Determine, if Generate function should randomize new seed.
    /// </summary>
    public bool useCustomSeed = false;

    public int seed;
    public float locationScaleFactor = 0.35f;

    public int chunkRadius = 3;
    public int chunksLimit = 20;

    public Vector2 gEmptyCentreSize = new Vector2(120, 120);
    public Cut cuttingSettings = 0;

    public float scalingFactor = 1.0f;
    /// <summary>
    /// Amount of enviro objects to place in each cell.
    /// </summary>
    public Vector2Int enviroObjectsInCell = new Vector2Int(0, 5);

    /// <summary>
    /// Chance of creating empty cell
    /// </summary>
    [HideInInspector] public SpawnChance emptyChance;

    /// <summary>
    /// Inform if all prefabs in list are valid
    /// </summary>
    public bool IsValid { get; private set; }

    /// <summary>
    /// Chances of spawning objecton specific index in prefab array
    /// </summary>
    [HideInInspector] public List<SpawnChance> locationSpawnChances = new List<SpawnChance>();
    [HideInInspector] public List<SpawnChance> enviroSpawnChances = new List<SpawnChance>();

    /// <summary>
    /// Gets location prefabs from prefab database without Application Manager
    /// </summary>
    public List<LocationsPool> Locations { get; private set; }
    public List<GameObject> Enviro { get; private set; }

    private List<Cell> generatedCells;
    private int lastCellHash;

    #endregion

    #region MonoBehaviour

    private void OnValidate() => Initialize();

    private void Awake() => Initialize();

    private void Update()
    {
        var currCell = GeneralGrid.GetNear(WorldSceneManager.Instance.Leader.transform.position)[0];
        var currCellHash = currCell.name.GetHashCode();

        if (lastCellHash != currCellHash)
        {
            Debug.Log($"{lastCellHash} =/= {currCellHash}");
            lastCellHash = currCellHash;
            Generate(currCell.transform.position, true);
        }
    }

    #endregion

    #region Component

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

    public void Initialize()
    {
        Locations = new List<LocationsPool>();
        Locations.AddRange(PrefabDatabase.Load.stdLocations);

        Enviro = new List<GameObject>();
        Enviro.AddRange(PrefabDatabase.Load.enviro);

        locationSpawnChances.Resize(Locations.Count, new SpawnChance());
        enviroSpawnChances.Resize(Enviro.Count, new SpawnChance());

        ValidatePrefabs();

        GeneralGrid.Validate();

        generatedCells = new List<Cell>();
    }

    [ContextMenu("Validate")]
    public void ValidatePrefabs()
    {
        foreach (LocationsPool pool in Locations)
        {
            foreach (var prefab in pool.locations)
            {
                int scriptsCount = prefab.GetComponentsInChildren<Location>().Length;

                if (scriptsCount != 1)
                {
                    IsValid = false;
                    throw new LocationValidationException(prefab, scriptsCount);
                }
            }
        }
        foreach (GameObject prefab in Enviro)
        {
            int scriptsCount = prefab.GetComponentsInChildren<EnviroObject>().Length;

            if (scriptsCount != 1)
            {
                IsValid = false;
                throw new LocationValidationException(prefab, scriptsCount);
            }
        }
        IsValid = true;
    }

    [ContextMenu("Generate")]
    public void Generate(Vector3 origin, bool radiusLimit)
    {
        if (!useCustomSeed)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }

        var locationRandomizer = new Roulette<LocationsPool>(Locations, locationSpawnChances, emptyChance);
        var enviroRandomizer = new Roulette<GameObject>(Enviro, enviroSpawnChances);

        var locInstances = new Stack<GameObject>();

        GeneralGrid.Generate(transform, transform.position, cuttingSettings);

        var newCells = new List<Cell>();

        if (radiusLimit)
        {
            var oldCells = generatedCells;
            var nearCells = GeneralGrid.GetNear(origin, chunkRadius);

            generatedCells = oldCells.Except(nearCells).ToList();
            generatedCells.AddRange(nearCells);

            //TODO: do it better
            newCells = nearCells.Except(oldCells).ToList();

            int overflow = generatedCells.Count - chunksLimit;
            if (overflow > 0)
            {
                for (int i = 0; i < overflow; i++)
                {
                    Destroy(generatedCells[i].gameObject);
                }
                generatedCells.RemoveRange(0, overflow);
            }
        }
        else
        {
            if (generatedCells != GeneralGrid.Cells)
            {
                generatedCells = GeneralGrid.Cells;
                newCells = generatedCells;
            }
        }

        foreach (var cell in newCells)
        {
            Random.InitState(seed * cell.gameObject.name.GetHashCode()); // determinism guarantion
            
            GenerateObject(cell, locationRandomizer, locInstances, true, true);

            int envObjects = Random.Range(enviroObjectsInCell.x, enviroObjectsInCell.y + 1);

            for (int k = 0; k < envObjects; k++)
            {
                GenerateObject(cell, enviroRandomizer, locInstances, false, false);
            }
        }

        if (!Mathf.Approximately(gEmptyCentreSize.magnitude, 0.0f))
        {
            FreeCentre();
        }

        LoadState();
        ClearSave();
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        foreach (var child in GetComponentsInChildren<Cell>())
        {
            DestroyImmediate(child.gameObject);
        }

        foreach (var child in GetComponentsInChildren<EnviroObject>())
        {
            DestroyImmediate(child.gameObject);
        }

        foreach (var child in GetComponentsInChildren<EnviroObject>())
        {
            DestroyImmediate(child.gameObject);
        }
    }

    private void GenerateObject<T>(Cell cell, Roulette<T> randomizer, Stack<GameObject> instances, bool limitOffset, bool addToInstances)
    {
        Vector2 maxOffset;

        if (limitOffset)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                maxOffset = (cell.size - Vector2.one * GameplayManager.Instance.lastLocationRadius ) / 2.0f;
            }
            else
            {
                maxOffset = (cell.size - Vector2.one * FindObjectOfType<GameplayManager>().lastLocationRadius) / 2.0f;
            }
            // DON"T FORGET TO CHANGE THE BUILD LINE ALSO!!!!
#else
            
            maxOffset = (cell.size - Vector2.one * GameplayManager.Instance.lastLocationRadius ) / 2.0f;
#endif
        }
        else
        {
            maxOffset = cell.size / 2.0f;
        }

        T randomObj = randomizer.GetRandom(cell.zone);

        if (randomObj is GameObject)
        {
            Spawn(randomObj as GameObject, cell.transform);
        }
        else if (randomObj is LocationsPool)
        {
            var pool = randomObj as LocationsPool;
            
            Spawn(pool.locations[Random.Range(0, pool.locations.Count)], cell.transform, true);
        }
        else if (randomObj != null)
        {
            Debug.LogError($"Type of randomizer: {randomizer.GetType()} is invalid, only supperted types is Roulette<GameObjct> and Roulette<LocationsPool>");
        }

        void Spawn(GameObject prefab, Transform parent, bool isLocation = false)
        {
            var locationPosition = new Vector3
            (
                cell.Position.x + Random.Range(-maxOffset.x, maxOffset.x),
                prefab.transform.position.y,
                cell.Position.y + Random.Range(-maxOffset.y, maxOffset.y)
            );

            var instance = Instantiate(prefab, locationPosition, Quaternion.identity, parent);
            if (isLocation)
                instance.transform.localScale *= locationScaleFactor;

            if (IntersectionTest(instance, instances))
            {
                instance.transform.localScale *= scalingFactor;

                if (addToInstances)
                {
                    instances.Push(instance);
                }

                if (instance.TryGetComponent(out Location loc))
                {
                    loc.id = cell.name.GetHashCode();
                    loc.generatedBy = this;
                }

                if (instance.TryGetComponent(out EnviroObject env))
                {
                    env.Randomize();
                }
            }
            else
            {
                SGUtils.SafeDestroy(instance);
            }

        }
    }

    private void FreeCentre()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if (Mathf.Abs(child.position.x - transform.position.x) < gEmptyCentreSize.x / 2.0f
             && Mathf.Abs(child.position.z - transform.position.z) < gEmptyCentreSize.y / 2.0f)
            {
                if (child.GetComponent<EnviroObstacle>())
                {
                    SGUtils.SafeDestroy(child.gameObject);
                }
                else if(child.GetComponent<Location>())
                {
                    SGUtils.SafeDestroy(child.gameObject);
                }
            }
        }
    }

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

#endregion
}

