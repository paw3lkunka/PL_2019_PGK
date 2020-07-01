using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockWallChunk : MonoBehaviour
{
    public int index = 0;
    public float width = 100;
    public Vector3 zeroPos = Vector3.zero;
    public Vector3 axis = Vector3.right;

    private void OnValidate()
    {
        axis.Normalize();
    }

    private void Update()
    {
        Vector3 cultPos = WorldSceneManager.Instance.cult.transform.position;
        Vector3 project = Vector3.Project(cultPos, axis);
        Vector3 central = FindNearestTo(project);
        transform.position = central + index * axis * width;
    }

    private Vector3 FindNearestTo(Vector3 point)
    {
        Vector3 nearest1 = zeroPos;
        Vector3 nearest2 = Vector3.positiveInfinity;

        while (Vector3.Distance(nearest1, point) < Vector3.Distance(nearest2, point))
        {
            nearest2 = nearest1;
            nearest1 += axis * width;
        }

        return nearest2;
    }
}
