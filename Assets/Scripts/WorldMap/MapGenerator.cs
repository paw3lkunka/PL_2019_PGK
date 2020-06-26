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

        public Vector3 corner(int i)
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

    List<Cell> cells = new List<Cell>();

    private void OnDrawGizmos()
    {
        foreach (var item in cells)
        {
            for (int i = 0; i < 4; i++)
            {
                Gizmos.DrawLine(item.corner(i), item.corner(i + 1));
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

    /// <summary>
    /// Should be near to locations size;
    /// </summary>
    public int aproxLocationsSize;

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
    [HideInInspector] public List<int> spawnChances = new List<int>();

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

        spawnChances.Resize(Locations.Count, 0);

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

        cells.Clear();

        Vector2 centralIndex = (cellsNumber - Vector2.one) / 2.0f;
        Vector3 newPos = transform.position;
        Vector2 cellSize = farCellSize;

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

                cells.Add(new Cell(newPos, cellSize));
            }
        }

        foreach (var cell in cells)
        {
            // Fix position
            cell.Position3 -= (new Vector3(farCellSize.x, 0, farCellSize.y) + newPos - transform.position) / 2;

            int randomNumber = Random.Range(0, range);
            int index = 0;

            while (index < Locations.Count && chances[index] < randomNumber)
            {
                randomNumber -= chances[index];
                index++;
            }

            var maxOffset = (cell.size - Vector2.one * aproxLocationsSize) / 2.0f;

            var locationPosition = new Vector3
            (
                cell.position.x + Random.Range(-maxOffset.x, maxOffset.x),
                index < Locations.Count ? Locations[index].transform.position.y : 0,
                cell.position.y + Random.Range(-maxOffset.y, maxOffset.y)
            );

            if (index < Locations.Count)
            {
                var obj = Instantiate(Locations[index], locationPosition, Quaternion.identity, transform);
                var loc = obj.GetComponent<Location>();

                loc.id = loc.transform.position.GetHashCode();
                loc.generatedBy = this;
            }

            int envObjects = Random.Range(enviroObjectsInCell.x, enviroObjectsInCell.y + 1);

            for (int k = 0; k < envObjects; k++)
            {
                GameObject prefab = Enviro[Random.Range(0, Enviro.Count)];

                var envPosition = new Vector3
                (
                    cell.position.x + Random.Range(cell.size.x / -2, cell.size.x / 2),
                    prefab.transform.position.y,
                    cell.position.y + Random.Range(cell.size.y / -2, cell.size.y / 2)
                );

                Instantiate(prefab, envPosition, Quaternion.identity, transform).GetComponent<EnviroObject>().Randomize();
            }

            if (forceEmptyCentre)
            {
                FreeCentre();
            }
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
        foreach (Location loc in transform.GetComponentsInChildren<Location>())
        {
            Vector2 halfSize = (Vector2)nearCellSize * 0.5f;
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
