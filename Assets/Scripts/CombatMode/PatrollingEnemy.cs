using UnityEngine;

public class PatrollingEnemy : Enemy
{
    #region Variables

#pragma warning disable
    [SerializeField]
    private Vector2[] patrolPoints;
#pragma warning restore

    private int destPoint = 0;

    #endregion

    #region MonoBehaviour

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        Agent.autoBraking = false;

        GotoNextPoint();
    }

    protected override void Update()
    {
        base.Update();

        if (ShouldPatrol())
        {
            GotoNextPoint();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        foreach (Vector2 p in patrolPoints)
        {
            Gizmos.DrawSphere(p, .2f);
        }
    }

    #endregion

    #region Component

    private void GotoNextPoint()
    {
        if (patrolPoints.Length == 0)
        {
            return;
        }

        Agent.destination = patrolPoints[destPoint];

        destPoint = (destPoint + 1) % patrolPoints.Length;
    }

    private bool ShouldPatrol()
    {
        return !chasedObject && !(Agent?.pathPending ?? false) && (Agent?.remainingDistance ?? float.PositiveInfinity) < 0.5f;
    }

    #endregion
}
