using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnviroObstacle : EnviroObject
{
    [ContextMenu("Randomize")]
    public override void Randomize()
    {
        transform.rotation = Quaternion.AngleAxis(Random.Range(0,360), Vector3.up);
    }
}
