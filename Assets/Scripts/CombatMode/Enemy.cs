using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Shooter))]
public class Enemy : Character
{
#pragma warning disable
    [SerializeField]
    private float eyesightRange;
    [SerializeField]
    private float shootingRange;
    [SerializeField]
    private float chaseRange;
#pragma warning restore

    protected GameObject chasedObject;
    protected Shooter shooter;

    protected override void Start()
    {
        base.Start();
        CombatSceneManager.Instance.enemies.Add(gameObject);
    }

    protected override void Awake()
    {
        base.Awake(); 

        shooter = GetComponent<Shooter>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    protected override void Update()
    {
        base.Update();

        var nearestDistance = CombatSceneManager.Instance.ourCrew.NearestFrom(transform.position);
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
            
            if(ShouldShoot())
            {
                agent.isStopped = true;
                shooter.target = chasedObject.transform.position;
                shooter.StartShooting();
                return;
            }

            shooter.StopShooting();

            if (ShouldChase())
            {
                agent.isStopped = false;
                agent.destination = chasedObject.transform.position;
                return;
            }

            chasedObject = null;
            agent.isStopped = false;
        }

    }

    public override void Die()
    {
        float gainedFaith = GameManager.Instance.FaithForKilledEnemy;
        GameManager.Instance.Faith += gainedFaith;
        emitter.Emit("+" + (int)(gainedFaith * 100), Color.green, 3);
        base.Die();
    }
    
    private bool ShouldShoot() => chasedObject ? Vector2.Distance(chasedObject.transform.position, this.transform.position) <= this.shootingRange : false;

    private bool ShouldChase() => chasedObject ? Vector2.Distance(chasedObject.transform.position, this.transform.position) < this.chaseRange : false;
}
