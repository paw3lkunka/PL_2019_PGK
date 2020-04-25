using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Moveable))]
public class BehaviourPatrol : MonoBehaviour, IBehaviour
{
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
        float distance = (transform.position - patrolPoints[index].position).magnitude;
        //TODO ignore y axis
        if ( distance < PatrolPointMatchDistance)
        {
            index++;
            if (index >= patrolPoints.Length)
            {
                index = 0;
            }
        }
    }
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
    #endregion

    public void UpdateTarget(Vector3? target)
    {
        moveable.Go(target ?? patrolPoints[index].position);
    }


#if UNITY_EDITOR
    public Color gizmoColor = Color.magenta;
#endif
}
