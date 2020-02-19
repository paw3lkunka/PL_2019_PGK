using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Shooter),typeof(DynamicObject))]
public class Enemy : Character
{
    #region Variables

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

    #endregion

    #region MonoBehaviour

    protected override void Awake()
    {
        base.Awake();

        shooter = GetComponent<Shooter>();

        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }

    protected override void Start()
    {
        base.Start();
        CrewSceneManager.Instance.enemies.Add(gameObject);
    }

    protected override void Update()
    {
        base.Update();

        var nearestDistance = GameManager.Instance.ourCrew.NearestFrom(transform.position);
        if (!nearestDistance.Item1)
        {
            return;
        }

        if (!chasedObject)
        {
            if (nearestDistance.Item2 < this.eyesightRange)
            {
                chasedObject = nearestDistance.Item1;
            }
        }
        else
        {
            chasedObject = nearestDistance.Item1;

            if (ShouldShoot())
            {
                Agent.isStopped = true;
                shooter.target = chasedObject.transform.position;
                shooter.StartShooting();
                return;
            }

            shooter.StopShooting();

            if (ShouldChase())
            {
                Agent.isStopped = false;
                Agent.destination = chasedObject.transform.position;
                return;
            }

            chasedObject = null;
            Agent.isStopped = false;
        }

    }

    #endregion

    #region Component

    public override void Die()
    {

        float gainedFaith = GameManager.Instance.FaithForKilledEnemy;
        GameManager.Instance.Faith += gainedFaith;

        if (fatihTextEemitter)
        {
            fatihTextEemitter.Emit("+" + (int)Mathf.Round(gainedFaith * 100), Color.green, 3);
        }
        GetComponent<DynamicObject>().RememberAsDestroyed();
        base.Die();
    }

    private bool ShouldShoot()
    {
        return chasedObject ? Vector2.Distance(chasedObject.transform.position, this.transform.position) <= this.shootingRange : false;
    }

    private bool ShouldChase()
    {
        return chasedObject ? Vector2.Distance(chasedObject.transform.position, this.transform.position) < this.chaseRange : false;
    }

    #endregion
}
