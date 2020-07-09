using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    [System.Flags] public enum Cut
    {
        XAxisP = 0b0001,
        XAxisN = 0b0010,
        ZAxisP = 0b0100,
        ZAxisN = 0b1000,
    }

    private class Cell
    {
        public Vector2 position;
        public Vector2 size;

        public Cell(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
        }
        public Cell(Vector3 position, Vector2 size)
        {
            this.position = new Vector2(position.x, position.z);
            this.size = size;
        }

        public Vector3 Position3 
        { 
            get => new Vector3(position.x, 0, position.y);
            set { position.x = value.x; position.y = value.z; }
        }

        public Vector3 Corner(int i)
        {
            switch(i % 4)
            {
                case 0:
                    return new Vector3(position.x + size.x / 2, 0, position.y + size.y / 2);
                case 1:
                    return new Vector3(position.x + size.x / 2, 0, position.y + size.y / -2);
                case 2:
                    return new Vector3(position.x + size.x / -2, 0, position.y + size.y / -2);
                case 3:
                    return new Vector3(position.x + size.x / -2, 0, position.y + size.y / 2);
            }

            throw new System.Exception("This should never happen.");
        }
    }

    private class Roulette
    {
        public Roulette(IList<GameObject> prefabs, IList<int> spawnChances, int emptyChance = 0)
        {
            this.prefabs = prefabs;
            this.chances = new List<int>();
            this.range = 0;

            for (int i = 0; i < prefabs.Count; i++)
            {
                int chance = spawnChances[i];
                range += chance;
                chances.Add(chance);
            }

            range += emptyChance;
            chances.Add(emptyChance);
        }

        public GameObject RandomizePrefab()
        {
            int randomNumber = Random.Range(0, range + 1);
            int index = 0;

            while (index < prefabs.Count && chances[index] < randomNumber)
            {
                randomNumber -= chances[index];
                index++;
            }

            return index < prefabs.Count ? prefabs[index] : null;
        }

        private IList<GameObject> prefabs;
        private List<int> chances;
        private int range;
    }


    List<Cell> cells = new List<Cell>();

    private void OnDrawGizmos()
    {
        foreach (var item in cells)
        {
            for (int i = 0; i < 4; i++)
            {
                Gizmos.DrawLine(item.Corner(i), item.Corner(i + 1));
            }
        }
    }

    #region Variables

    /// <summary>
    /// Determine, if Generate function should randomize new seed.
    /// </summary>
    public bool useCustomSeed = false;

    /// <summary>
    /// Guarantee, that cente of map will be empty
    /// </summary>
    public bool forceEmptyCentre = false;

    public int seed;
        
    /// <summary>
    /// Size of grid cells
    /// </summary>
    public Vector2Int cellsNumber = new Vector2Int(5, 5);

    /// <summary>
    /// Size of single cell
    /// </summary>
    public Vector2 nearCellSize = new Vector2(30, 30);

    public Vector2 farCellSize = new Vector2(30, 30);

    public Cut cuttingSettings = 0;

    public float scalingFactor = 1.0f;
    /// <summary>
    /// Amount of enviro objects to place in each cell.
    /// </summary>
    public Vector2Int enviroObjectsInCell = new Vector2Int(0, 5);

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
    [HideInInspector] public List<int> locationSpawnChances = new List<int>();
    [HideInInspector] public List<int> enviroSpawnChances = new List<int>();

    /// <summary>
    /// Gets location prefabs from prefab database without Application Manager
    /// </summary>
    public List<GameObject> Locations { get; private set; }


    public List<GameObject> Enviro { get; private set; }

    #endregion

    #region MonoBehaviour

    private void OnValidate() => Initialize();

    private void Awake() => Initialize();

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

    private void Initialize()
    {
        Locations = new List<GameObject>();
        Locations.AddRange(PrefabDatabase.Load.stdLocations);
        Locations.AddRange(PrefabDatabase.Load.shrines);

        Enviro = new List<GameObject>();
        Enviro.AddRange(PrefabDatabase.Load.enviro);

        locationSpawnChances.Resize(Locations.Count, 0);
        enviroSpawnChances.Resize(Enviro.Count, 0);

        ValidatePrefabs();

        cellsNumber = Vector2Int.Max(cellsNumber, Vector2Int.one);
        nearCellSize = Vector2.Max(nearCellSize, Vector2.zero);
        farCellSize = Vector2.Max(farCellSize, Vector2.zero);
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
    public void Generate()
    {
        if (!useCustomSeed)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }

        Random.InitState(seed);

        var locationRandomizer = new Roulette(Locations, locationSpawnChances, emptyChance);
        var enviroRandomizer = new Roulette(Enviro, enviroSpawnChances);

        cells.Clear();

        Vector2 centralIndex = (cellsNumber - Vector2.one) / 2.0f;
        Vector3 newPos = transform.position;
        Vector2 cellSize = farCellSize;

        var locInstances = new Stack<GameObject>();

        for (int i = 0; i < cellsNumber.x; i++)
        {
            newPos.z = transform.position.z;

            newPos.x += cellSize.x / 2;
            cellSize.x = Mathf.Lerp(nearCellSize.x, farCellSize.x, Mathf.Abs(centralIndex.x - i) / centralIndex.x);
            newPos.x += cellSize.x / 2;

            for (int j = 0; j < cellsNumber.y; j++)
            {
                newPos.z += cellSize.y / 2;                
                cellSize.y = Mathf.Lerp(nearCellSize.y, farCellSize.y, Mathf.Abs(centralIndex.y - j) / centralIndex.y);
                newPos.z += cellSize.y / 2;

                if (!ShouldBeCutOff(i,j))
                {
                    cells.Add(new Cell(newPos, cellSize));
                }
            }
        }

        foreach (var cell in cells)
        {
            // Fix position
            cell.Position3 -= (new Vector3(farCellSize.x, 0, farCellSize.y) + newPos - transform.position) / 2;
#if UNITY_EDITOR
            var maxOffset = (cell.size - Vector2.one * FindObjectOfType<GameplayManager>().lastLocationRadius) / 2.0f;
#else
            var maxOffset = (cell.size - Vector2.one * GameplayManager.Instance.lastLocationRadius ) / 2.0f;
#endif
            GameObject locPrefab = locationRandomizer.RandomizePrefab();

            if (locPrefab)
            {
                var locationPosition = new Vector3
                (
                    cell.position.x + Random.Range(-maxOffset.x, maxOffset.x),
                    locPrefab.transform.position.y,
                    cell.position.y + Random.Range(-maxOffset.y, maxOffset.y)
                );

                locInstances.Push(Instantiate(locPrefab, locationPosition, Quaternion.identity, transform));
                locInstances.Peek().transform.localScale *= scalingFactor;
                var loc = locInstances.Peek().GetComponent<Location>();

                loc.id = loc.transform.position.GetHashCode();
                loc.generatedBy = this;
            }
        }

        foreach (var cell in cells)
        {
            int envObjects = Random.Range(enviroObjectsInCell.x, enviroObjectsInCell.y + 1);

            for (int k = 0; k < envObjects; k++)
            {
                GameObject envPrefab = enviroRandomizer.RandomizePrefab();

                var envPosition = new Vector3
                (
                    cell.position.x + Random.Range(cell.size.x / -2, cell.size.x / 2),
                    envPrefab.transform.position.y,
                    cell.position.y + Random.Range(cell.size.y / -2, cell.size.y / 2)
                );

                var envObject = Instantiate(envPrefab, envPosition, Quaternion.identity, transform);

                if (EnvIntersectionTest(envObject, locInstances))
                {
                    envObject.transform.localScale *= scalingFactor;
                    envObject.GetComponent<EnviroObject>().Randomize();
                }
                else
                {
                    //Debug.Log("Obstacle removed due to collision with location");
                    SGUtils.SafeDestroy(envObject);
                }
            }
        }

        if (forceEmptyCentre)
        {
            FreeCentre();
        }

        LoadState();
        ClearSave();
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        foreach (var child in GetComponentsInChildren<Location>())
        {
            DestroyImmediate(child.gameObject);
        }

        foreach (var child in GetComponentsInChildren<EnviroObject>())
        {
            DestroyImmediate(child.gameObject);
        }
    }

    private void FreeCentre()
    {
        float radius = Mathf.Sqrt(nearCellSize.x * nearCellSize.x + nearCellSize.y + nearCellSize.y) / 2;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if (Vector3.Distance(transform.position, child.position) < radius)
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

    private bool ShouldBeCutOff(int x, int y)
    {
        return x * 2 > cellsNumber.x && (cuttingSettings & Cut.XAxisP) != 0
            || x * 2 < cellsNumber.x && (cuttingSettings & Cut.XAxisN) != 0
            || y * 2 > cellsNumber.y && (cuttingSettings & Cut.ZAxisP) != 0
            || y * 2 < cellsNumber.y && (cuttingSettings & Cut.ZAxisN) != 0;
    }

    private bool EnvIntersectionTest(GameObject envObject, IEnumerable<GameObject> locInstances)
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

