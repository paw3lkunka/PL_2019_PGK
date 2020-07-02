using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class EnviroObstacle : EnviroObject
{
    [ContextMenu("Randomize")]
    public override void Randomize()
    {
        transform.rotation = Quaternion.AngleAxis(Random.Range(0,360), Vector3.up);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
