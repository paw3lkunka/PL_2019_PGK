using System.Collections.Generic;
using System.Linq;
using System.Security;
using UnityEngine;

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

    public class Cell
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
    
    [System.Serializable]
    public class Grid
    {
        public List<Cell> Cells { get; private set; } = new List<Cell>();

        /// <summary>
        /// Size of grid cells
        /// </summary>
        public Vector2Int CellsNumber = new Vector2Int(20, 20);

        /// <summary>
        /// Size of single cell
        /// </summary>
        public Vector2 NearCellSize = new Vector2(100, 100);

        public Vector2 FarCellSize = new Vector2(150, 150);

        public Vector2 CentralIndex { get => (CellsNumber - Vector2.one) / 2.0f; }

        public void Validate()
        {
            CellsNumber = Vector2Int.Max(CellsNumber, Vector2Int.one);
            NearCellSize = Vector2.Max(NearCellSize, Vector2.zero);
            FarCellSize = Vector2.Max(FarCellSize, Vector2.zero);
        }

        public void Generate(Vector3 position, Cut cuttingSettings)
        {
            Cells.Clear();

            Vector3 newPos = position;
            Vector2 cellSize = FarCellSize;

            for (int i = 0; i < CellsNumber.x; i++)
            {
                newPos.z = position.z;

                newPos.x += cellSize.x / 2;
                cellSize.x = Mathf.Lerp(NearCellSize.x, FarCellSize.x, Mathf.Abs(CentralIndex.x - i) / CentralIndex.x);
                newPos.x += cellSize.x / 2;

                for (int j = 0; j < CellsNumber.y; j++)
                {
                    newPos.z += cellSize.y / 2;
                    cellSize.y = Mathf.Lerp(NearCellSize.y, FarCellSize.y, Mathf.Abs(CentralIndex.y - j) / CentralIndex.y);
                    newPos.z += cellSize.y / 2;

                    if (!ShouldBeCutOff(cuttingSettings, i, j))
                    {
                        Cells.Add(new Cell(newPos, cellSize));
                    }
                }
            }

            // Fix position
            foreach (var cell in Cells)
            {
                cell.Position3 -= (new Vector3(FarCellSize.x, 0, FarCellSize.y) + Cells.Last().Position3 - position) / 2;
            }
        }

        private bool ShouldBeCutOff(Cut cuttingSettings, int x, int y)
        {
            return x * 2 > CellsNumber.x && (cuttingSettings & Cut.XAxisP) != 0
                || x * 2 < CellsNumber.x && (cuttingSettings & Cut.XAxisN) != 0
                || y * 2 > CellsNumber.y && (cuttingSettings & Cut.ZAxisP) != 0
                || y * 2 < CellsNumber.y && (cuttingSettings & Cut.ZAxisN) != 0;
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

    [field: SerializeField] public Grid GeneralGrid { get; private set; }
    [field: SerializeField] public Grid ShrinesGrid { get; private set; }

    private void OnDrawGizmos()
    {
        Color saveColor = Gizmos.color;
        Gizmos.color = Color.black;
        foreach (var item in ShrinesGrid.Cells)
        {
            for (int i = 0; i < 4; i++)
            {
                Gizmos.DrawLine(item.Corner(i), item.Corner(i + 1));
            }
        }

        Gizmos.color = Color.white;
        foreach (var item in GeneralGrid.Cells)
        {
            for (int i = 0; i < 4; i++)
            {
                Gizmos.DrawLine(item.Corner(i), item.Corner(i + 1));
            }
        }
        Gizmos.color = saveColor;
    }

    #region Variables

    /// <summary>
    /// Determine, if Generate function should randomize new seed.
    /// </summary>
    public bool useCustomSeed = false;

    public int seed;
        

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
    [HideInInspector] public int emptyChance;

    /// <summary>
    /// Inform if all prefabs in list are valid
    /// </summary>
    public bool IsValid { get; private set; }

    /// <summary>
    /// Chances of spawning objecton specific index in prefab array
    /// </summary>
    [HideInInspector] public List<int> locationSpawnChances = new List<int>();
    [HideInInspector] public List<int> shrinesSpawnChances = new List<int>();
    [HideInInspector] public List<int> enviroSpawnChances = new List<int>();

    /// <summary>
    /// Gets location prefabs from prefab database without Application Manager
    /// </summary>
    public List<GameObject> Locations { get; private set; }
    public List<GameObject> Shrines { get; private set; }
    public List<GameObject> Enviro { get; private set; }

    private int nextId;

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

        Shrines = new List<GameObject>();
        Shrines.AddRange(PrefabDatabase.Load.shrines);

        Enviro = new List<GameObject>();
        Enviro.AddRange(PrefabDatabase.Load.enviro);

        locationSpawnChances.Resize(Locations.Count, 0);
        shrinesSpawnChances.Resize(Locations.Count, 0);
        enviroSpawnChances.Resize(Enviro.Count, 0);

        ValidatePrefabs();

        GeneralGrid.Validate();
        ShrinesGrid.Validate();
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
        nextId = 1;

        if (!useCustomSeed)
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
        }

        Random.InitState(seed);

        var locationRandomizer = new Roulette(Locations, locationSpawnChances, emptyChance);
        var shrineRandomizer = new Roulette(Shrines, shrinesSpawnChances);
        var enviroRandomizer = new Roulette(Enviro, enviroSpawnChances);

        var locInstances = new Stack<GameObject>();

        GeneralGrid.Generate(transform.position, cuttingSettings);
        ShrinesGrid.Generate(transform.position, cuttingSettings);

        foreach (var cell in ShrinesGrid.Cells)
        {
            GenerateObject(cell, shrineRandomizer, locInstances, true, true);
        }

        foreach (var cell in GeneralGrid.Cells)
        {
            GenerateObject(cell, locationRandomizer, locInstances, true, true);
        }

        foreach (var cell in GeneralGrid.Cells)
        {
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
        foreach (var child in GetComponentsInChildren<Location>())
        {
            DestroyImmediate(child.gameObject);
        }

        foreach (var child in GetComponentsInChildren<EnviroObject>())
        {
            DestroyImmediate(child.gameObject);
        }
    }

    void GenerateObject(Cell cell, Roulette randomizer, Stack<GameObject> instances, bool limitOffset, bool addToInstances)
    {
        Vector2 maxOffset;

        if (limitOffset)
        {
#           if UNITY_EDITOR
                maxOffset = (cell.size - Vector2.one * FindObjectOfType<GameplayManager>().lastLocationRadius) / 2.0f;
#           else
                maxOffset = (cell.size - Vector2.one * GameplayManager.Instance.lastLocationRadius ) / 2.0f;
#           endif
        }
        else
        {
            maxOffset = cell.size / 2.0f;
        }

        GameObject prefab = randomizer.RandomizePrefab();

        if (prefab)
        {
            var locationPosition = new Vector3
            (
                cell.position.x + Random.Range(-maxOffset.x, maxOffset.x),
                prefab.transform.position.y,
                cell.position.y + Random.Range(-maxOffset.y, maxOffset.y)
            );

            var instance = Instantiate(prefab, locationPosition, Quaternion.identity, transform);

            if (IntersectionTest(instance, instances))
            {
                instance.transform.localScale *= scalingFactor;

                if (addToInstances)
                {
                    instances.Push(instance);
                }

                if (instance.TryGetComponent(out Location loc))
                {
                    loc.id = nextId++;
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

