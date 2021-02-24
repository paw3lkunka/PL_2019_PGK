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

    public Vector2 size;

    public int zone = 0;
    public bool spawned = false;


    public void Set(Vector2 position, Vector2 size)
    {
        this.Position = position;
        this.size = size;
    }

    public void Set(Vector3 position, Vector2 size)
    {
        this.Position = new Vector2(position.x, position.z);
        this.size = size;
    }

    public Vector3 Corner(int i)
    {
        switch (i % 4)
        {
            case 0:
                return new Vector3(Position.x + size.x / 2, 0, Position.y + size.y / 2);
            case 1:
                return new Vector3(Position.x + size.x / 2, 0, Position.y + size.y / -2);
            case 2:
                return new Vector3(Position.x + size.x / -2, 0, Position.y + size.y / -2);
            case 3:
                return new Vector3(Position.x + size.x / -2, 0, Position.y + size.y / 2);
        }

        throw new System.Exception("This should never happen.");
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
