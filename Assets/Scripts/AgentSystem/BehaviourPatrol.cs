using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Moveable))]
public class BehaviourPatrol : MonoBehaviour, IBehaviour
{
    [Tooltip("Distance 2D in XZ axes needet tho match patrol point as visited.")]
    public float PatrolPointMatchDistance = 0.5f;
#pragma warning disable
    [SerializeField] private Transform[] patrolPoints = new Transform[0];
#pragma warning restore
    private Moveable moveable;
    [SerializeField]
    private int index;

    #region MonoBehaviour

    private void Awake()
    {
        moveable = GetComponent<Moveable>();
        index = 0;
    }

    private void FixedUpdate()
    {
        Vector3 difference = transform.position - patrolPoints[index].position;
        difference.y = 0;
        if (difference.magnitude < PatrolPointMatchDistance)
        {
            index++;
            if (index >= patrolPoints.Length)
            {
                index = 0;
            }
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
    public Color gizmoColor = Color.magenta;
#endif
    #endregion

    public void UpdateTarget(Vector3? target)
    {
        moveable.Go(target ?? patrolPoints[index].position);
    }
}
