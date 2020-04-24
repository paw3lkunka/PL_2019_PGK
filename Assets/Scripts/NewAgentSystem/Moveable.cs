using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Moveable : MonoBehaviour
{
    [Flags]
    public enum Flags
    {
        canMove = 0b0000_0001,
    }

    public Flags flags;

    public virtual float SpeedBase 
    { get => navMeshAgent.speed; set => navMeshAgent.speed = value; }

    [SerializeField]
    private float speedBase;


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
        SpeedBase = speedBase;
    }

    protected void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    #endregion
}
