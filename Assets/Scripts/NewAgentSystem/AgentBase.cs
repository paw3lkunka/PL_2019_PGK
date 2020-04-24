using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //TODO replace with cultist list reference
    public IEnumerable<Vector3> MOCKenemies;

    public enum DetectionStrategy
    {
        none,
        axidental,
        nearest,
        farthest,
        //TODO implement
        random,
        strongest,
        weakest,
    }

    public DetectionStrategy detectionStrategy;

    [field: SerializeField, GUIName("DetectionRange")]
    public float DetectionRange { get; private set; }

    private Shooting shooting;
    private Moveable moveable;

    public Func<Vector3?> Detection
    {
        get
        {
            Func<Vector3?> func;
            switch (detectionStrategy)
            {
                case DetectionStrategy.none:
                    func = () => null;
                    break;
                case DetectionStrategy.axidental:
                    func = AxidentalStartegy;
                    break;
                case DetectionStrategy.nearest:
                    func = NearestStartegy;
                    break;
                case DetectionStrategy.farthest:
                    func = FarthestStartegy;
                    break;
                case DetectionStrategy.random:
                    func = () => throw new NotImplementedException();
                    break;
                case DetectionStrategy.strongest:
                    func = () => throw new NotImplementedException();
                    break;
                case DetectionStrategy.weakest:
                    func = () => throw new NotImplementedException();
                    break;
                default:
                    func = () => throw new NotImplementedException();
                    break;
            }
            return func;
        }
    }

    #region MonoBehaviour

    private void Start()
    {
        moveable = GetComponent<Moveable>();
        shooting = GetComponent<Shooting>();
    }

    private void FixedUpdate()
    {
        moveable
    }

    #endregion

    #region Strategies
    public Vector3? AxidentalStartegy()
    {
        foreach (Vector3 enemyPos in MOCKenemies)
        {
            if ((enemyPos - transform.position).magnitude < DetectionRange)
            {
                return enemyPos;
            }
        }
        return null;
    }
    public Vector3? NearestStartegy()
    {
        float distance = DetectionRange;
        Vector3? target = null;
        foreach (Vector3 enemyPos in MOCKenemies)
        {
            if ((enemyPos - transform.position).magnitude < distance)
            {
                target = enemyPos;
            }
        }
        return target;
    }
    public Vector3? FarthestStartegy()
    {
        float distance = 0;
        Vector3? target = null;
        foreach (Vector3 enemyPos in MOCKenemies)
        {
            float currDistance = (enemyPos - transform.position).magnitude;
            if (currDistance > distance && currDistance < DetectionRange)
            {
                target = enemyPos;
            }
        }
        return target;
    }
    #region
}
