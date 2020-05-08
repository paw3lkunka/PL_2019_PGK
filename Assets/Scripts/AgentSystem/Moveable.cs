using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Moveable : MonoBehaviour
{
    [Flags]
    public enum Flags
    {
        nothing = 0,
        canMove = 0b0000_0001,
    }

    public Flags flags = Flags.canMove;


    [field: SerializeField, GUIName("SpeedBase")]
    public float SpeedBase { get; set; }


    /// <summary>
    /// Set agents destination if it can move.
    /// </summary>
    /// <param name="target">agents destination.</param>
    /// <returns>If destination was setted, ergo, if can move.</returns>
    public bool Go(Vector3 target)
    {
        if( (flags & Flags.canMove) != 0 )
        {
            navMeshAgent.SetDestination(target);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Imidietlly stops agent, resetting its target.
    /// </summary>
    public void Stop()
    {
        navMeshAgent.SetDestination(transform.position);
    }


    #region MonoBehaviour

    protected NavMeshAgent navMeshAgent;

    protected void OnValidate()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = SpeedBase;
    }

    protected void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
    }

    protected void Update()
    {
        navMeshAgent.speed = SpeedBase;
    }
    #endregion
}
