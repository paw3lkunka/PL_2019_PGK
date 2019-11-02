using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Shooter))]
public abstract class Enemy : Character
{
#pragma warning disable
    [SerializeField]
    private float eyesightRange;
    [SerializeField]
    private float shootingRange;
    [SerializeField]
    private float chaseRange;
#pragma warning restore

    protected NavMeshAgent agent;
    protected GameObject chasedObject;
    protected Shooter shooterComponent;

    protected virtual void Awake()
    {
        shooterComponent = GetComponent<Shooter>();
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.autoBraking = false;
    }

    protected override void Update()
    {
        base.Update();

        var nearestDistance = GameManager.Instance.ourCrew.NearestFrom(transform.position);
        if(!nearestDistance.Item1)
        {
            return;
        }

        if(!chasedObject)
        {
            if(nearestDistance.Item2 < this.eyesightRange)
            {
                chasedObject = nearestDistance.Item1;
            }
        }
        else
        {
            chasedObject = nearestDistance.Item1;
            
            if(IsShooting())
            {
                agent.isStopped = true;
                shooterComponent.target = chasedObject.transform.position;
                shooterComponent.StartShooting();
                return;
            }

            shooterComponent.StopShooting();

            if (IsChasing())
            {
                agent.isStopped = false;
                agent.destination = chasedObject.transform.position;
                return;
            }

            chasedObject = null;
            agent.isStopped = false;
        }
    }

    private bool IsShooting() => chasedObject ? Vector2.Distance(chasedObject.transform.position, this.transform.position) <= this.shootingRange : false;

    private bool IsChasing() => chasedObject ? Vector2.Distance(chasedObject.transform.position, this.transform.position) > this.chaseRange : false;
}
