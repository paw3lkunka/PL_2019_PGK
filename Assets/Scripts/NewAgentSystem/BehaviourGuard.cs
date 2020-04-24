using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Moveable), typeof(Shooting))]
public class BehaviourGuard : MonoBehaviour
{
    [field: SerializeField, GUIName("ChaseRange")]
    public float ChaseRange { get; private set; }

    public Vector3 post;

    private Moveable moveable;

    public void SetPostHere() => post = transform.position;

    #region MonoBehaviour

    private void Awake()
    {
        moveable = GetComponent<Moveable>();
    }

    #endregion
}