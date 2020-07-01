using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockWallChunk : MonoBehaviour
{
    protected bool isOriginal = true;
    public int instances = 3;
    public int index = 0;
    public float width = 100;
    public Vector3 zeroPos = Vector3.zero;
    public Vector3 axis = Vector3.right;
    public bool enableTeleportation = true;

    private void OnValidate()
    {
        axis.Normalize();
    }

    private void Start()
    {
        //Will blow up - if will be in awake
        if (isOriginal)
        {
            GeneratreInstances();
        }
        Teleport();
    }

    private void Update()
    {
        if (enableTeleportation)
        {
            Teleport();
        }
    }

    public void GeneratreInstances()
    {        
        for (int i = 0; i < instances; i++ )
        {
            var clone = Instantiate(gameObject, transform.parent).GetComponent<RockWallChunk>();
            clone.isOriginal = false;
            clone.index = i % 2 != 0 ? i / 2 : -i / 2;
        }
    }

    private void Teleport()
    {
        Vector3 cultPos = WorldSceneManager.Instance?.cult.transform.position ?? zeroPos;
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
