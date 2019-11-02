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
        GotoNextPoint();
    }

    protected override void Update()
    {
        base.Update();

        if(!chasedObject && !agent.pathPending && agent.remainingDistance < 0.5f)
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
}
