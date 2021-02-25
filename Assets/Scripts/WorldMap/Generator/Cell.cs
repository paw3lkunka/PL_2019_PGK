using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2 Position
    {
        get => new Vector2(transform.position.x, transform.position.z);
        set => transform.position = new Vector3(value.x, transform.position.y, value.y);
    }

    Grid grid;
    public int zone = 0;
    public bool spawned = false;

    public void Set(Grid grid, Vector2 position)
    {
        this.grid = grid;
        this.Position = position;
    }

    public void Set(Grid grid, Vector3 position)
    {
        this.grid = grid;
        position.y = transform.position.y;
        transform.position = position;
    }

    public Vector3 Corner(int i)
    {
        var size = grid.cellSize;

        return (i % 4) switch
        {
            0 => new Vector3(Position.x + size / 2, 0, Position.y + size / 2),
            1 => new Vector3(Position.x + size / 2, 0, Position.y + size / -2),
            2 => new Vector3(Position.x + size / -2, 0, Position.y + size / -2),
            3 => new Vector3(Position.x + size / -2, 0, Position.y + size / 2),
            _ => throw new System.Exception("This should never happen."),
        };
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawLine(Corner(i), Corner(i + 1));
        }
    }
}
