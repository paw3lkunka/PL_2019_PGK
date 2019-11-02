using UnityEngine;

public class PatrollingEnemy : Enemy
{
#pragma warning disable
    [SerializeField]
    private Vector2[] points;
#pragma warning restore

    private int destPoint = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
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
        if(points.Length == 0)
        {
            return;
        }

        agent.destination = points[destPoint];

        destPoint = (destPoint + 1) % points.Length;
    }
}
