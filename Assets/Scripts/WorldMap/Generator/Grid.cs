using System.Linq;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Grid
{
    public Cell[,] Cells { get; private set; }

    public int cellsInRow = 50;
    public float cellSize = 150;

    public float[] zonesBounds = new float[MapGenerator.ZONES - 1];

    public Vector2 CentralIndex { get => (Vector2.one * cellsInRow - Vector2.one) / 2.0f; }


    public void Generate(Transform parent, Vector3 position)
    {
        Cells = new Cell[cellsInRow, cellsInRow];

        Vector3 newPos = position;

        for (int i = 0; i < cellsInRow; i++)
        {
            newPos.z = position.z;

            newPos.x += cellSize;

            for (int j = 0; j < cellsInRow; j++)
            {
                newPos.z += cellSize;

                var gobj = new GameObject();
                gobj.transform.SetParent(parent);
                gobj.name = $"cell {i} {j}";

                var cell = gobj.AddComponent(typeof(Cell)) as Cell;
                cell.Set(this, newPos);
                Cells[i,j] = cell;
            }
        }

        // Fix position
        foreach (var cell in Cells)
        {
            cell.transform.position -= (new Vector3(cellSize, 0, cellSize) + Cells[cellsInRow - 1, cellsInRow - 1].transform.position - position) / 2;

            int zone = 0;
            float distance = Vector3.Distance(cell.transform.position, position);

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

    public Vector2Int GetNear(Vector3 point)
    {
        var ret = new Vector2Int(0, 0);

        for (int i = 0; i < cellsInRow; i++)
        {

            for (int j = 0; j < cellsInRow; j++)
            {
                if (Vector3.Distance(point, Cells[i, j].transform.position) < Vector3.Distance(point, Cells[ret.x, ret.y].transform.position))
                {
                    ret.x = i;
                    ret.y = j;
                }
            }
        }

        return ret;
    }
}
