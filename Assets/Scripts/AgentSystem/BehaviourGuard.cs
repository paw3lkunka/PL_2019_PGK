using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Moveable), typeof(IAttack))]
public class BehaviourGuard : MonoBehaviour, IBehaviour
{
    public Vector3 post;

    private Moveable moveable;

    /// <summary>
    /// Imidietly sets post on current position
    /// </summary>
    public void SetPostHere() => post = transform.position;

    /// <summary>
    /// Order to go to target, or post if target is null
    /// </summary>
    /// <param name="target">will be replaced by post if is null</param>
    public void UpdateTarget(Vector3? target)
    {
        moveable.Go(target ?? post);
    }

    #region MonoBehaviour

    private void Awake()
    {
        moveable = GetComponent<Moveable>();
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor; 
        Gizmos.DrawSphere(post, 0.5f);
    }
    public Color gizmoColor = Color.magenta;
#endif
    #endregion
}