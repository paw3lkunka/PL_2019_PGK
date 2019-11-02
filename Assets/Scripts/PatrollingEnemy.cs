using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PatrollingEnemy : Enemy
{
#pragma warning disable
    [SerializeField]
    private Vector2[] points;
#pragma warning restore

    private int destPoint = 0;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.autoBraking = false;
        GotoNextPoint();
    }

    private void Update()
    {
        base.Update();

        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }

    }

    private void GotoNextPoint()
    {
        if(points.Length == 0)
        {
            return;
        }

        agent.destination = (Vector3)points[destPoint];

        destPoint = (destPoint + 1) % points.Length;
    }

}
