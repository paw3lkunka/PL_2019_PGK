﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;


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

    public float[] zonesBounds = new float[MapGenerator.ZONES - 1];

    public Vector2 CentralIndex { get => (CellsNumber - Vector2.one) / 2.0f; }

    public void Validate()
    {
        CellsNumber = Vector2Int.Max(CellsNumber, Vector2Int.one);
        NearCellSize = Vector2.Max(NearCellSize, Vector2.zero);
        FarCellSize = Vector2.Max(FarCellSize, Vector2.zero);
    }

    public void Generate(Transform parent, Vector3 position, Cut cuttingSettings)
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
                    var gobj = MonoBehaviour.Instantiate(new GameObject(), parent);
                    gobj.name = $"cell {i} {j}";
                    var cell = gobj.AddComponent(typeof(Cell)) as Cell;
                    cell.Set(newPos, cellSize);
                    Cells.Add(cell);
                }
            }
        }

        // Fix position
        foreach (var cell in Cells)
        {
            cell.Position3 -= (new Vector3(FarCellSize.x, 0, FarCellSize.y) + Cells.Last().Position3 - position) / 2;

            int zone = 0;
            float distance = Vector3.Distance(cell.Position3, position);

            foreach (var bound in zonesBounds)
            {
                if (distance > bound)
                    zone++;
                else
                    break;
            }

            cell.zone = zone;
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
