using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2 position;
    public Vector2 size;

    public int zone = 0;

    public void Set(Vector2 position, Vector2 size)
    {
        this.position = position;
        this.size = size;
    }
    public void Set(Vector3 position, Vector2 size)
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
        switch (i % 4)
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
