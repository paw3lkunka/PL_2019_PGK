using UnityEngine;

public class PatrollingEnemy : Enemy
{
#pragma warning disable
    [SerializeField]
    private Vector2[] patrolPoints;
#pragma warning restore

    private int destPoint = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        agent.autoBraking = false;

        GotoNextPoint();
    }

    protected override void Update()
    {
        base.Update();

        if(ShouldPatrol())
        {
            GotoNextPoint();
        }
    }

    private void GotoNextPoint()
    {
        if(patrolPoints.Length == 0)
        {
            return;
        }

        agent.destination = patrolPoints[destPoint];

        destPoint = (destPoint + 1) % patrolPoints.Length;
    }

    private bool ShouldPatrol() => !chasedObject && !(agent?.pathPending ?? false) && (agent?.remainingDistance ?? float.PositiveInfinity) < 0.5f;
}
