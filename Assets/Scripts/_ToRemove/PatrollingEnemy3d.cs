using UnityEngine;

public class PatrollingEnemy3d : Enemy3d
{
    #region Variables

#pragma warning disable
    [SerializeField] private Transform[] patrolPoints;
#pragma warning restore

#if UNITY_EDITOR
    public Color gizmoColor = Color.magenta;
#endif

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

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        if (patrolPoints.Length > 0)
        {
            foreach (var p in patrolPoints)
            {
                Gizmos.DrawSphere(p.position, 0.5f);
            }
        }
    }
#endif

#endregion

    #region Component

    private void GotoNextPoint()
    {
        if (patrolPoints.Length == 0)
        {
            return;
        }

        Agent.destination = patrolPoints[destPoint].position;

        destPoint = (destPoint + 1) % patrolPoints.Length;
    }

    private bool ShouldPatrol()
    {
        return !chasedObject && !(Agent?.pathPending ?? false) && (Agent?.remainingDistance ?? float.PositiveInfinity) < 0.5f;
    }

    #endregion
}
