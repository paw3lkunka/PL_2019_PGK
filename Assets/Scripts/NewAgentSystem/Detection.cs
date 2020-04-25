using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    //TODO replace with cultist list reference
    public Transform[] MOCKEnemies = new Transform[0];


    /// <summary>
    /// Determine detection strategy.
    /// </summary>
    public enum DetectionStrategy
    {
        /// <summary>
        /// Can not detect anything.
        /// </summary>
        none,
        /// <summary>
        /// Returns first matching target in detection range.
        /// </summary>
        accidental,
        /// <summary>
        /// Returns random target from detection range.
        /// </summary>
        random, // TODO not implemented
        /// <summary>
        /// Returns nearest target in detection range.
        /// </summary>
        nearest,
        /// <summary>
        /// Returns farthest target in detection range.
        /// </summary>
        farthest,
        /// <summary>
        /// Returns target with greatest amount of heaktg in detection range.
        /// </summary>
        strongest, // TODO not implemented
        /// <summary>
        /// Returns target with smallest amount of heaktg in detection range.
        /// </summary>
        weakest, // TODO not implemented
    }

    /// <summary>
    /// Determine detection strategy.
    /// </summary>
    [field: SerializeField, GUIName("Strategy")]
    public DetectionStrategy Strategy { get; private set; } = DetectionStrategy.accidental;

    [field: SerializeField, GUIName("DetectionRange")]
    public float DetectionRange { get; private set; } = 10;


    /// <summary>
    /// Detection function depending on strategy.
    /// </summary>
    public Func<Vector3?> Func
    {
        get
        {
            Func<Vector3?> func;
            switch (Strategy)
            {
                case DetectionStrategy.none:
                    func = () => null;
                    break;
                case DetectionStrategy.accidental:
                    func = AccidentalStartegy;
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

    #region Strategies
    private Vector3? AccidentalStartegy()
    {
        foreach (var enemy in MOCKEnemies)
        {
            if ((enemy.position - transform.position).magnitude < DetectionRange)
            {
                return enemy.position;
            }
        }
        return null;
    }
    private Vector3? NearestStartegy()
    {
        float distance = DetectionRange;
        Vector3? target = null;
        foreach (var enemy in MOCKEnemies)
        {
            if ((enemy.position - transform.position).magnitude < distance)
            {
                target = enemy.position;
            }
        }
        return target;
    }
    private Vector3? FarthestStartegy()
    {
        float distance = 0;
        Vector3? target = null;
        foreach (var enemy in MOCKEnemies)
        {
            float currDistance = (enemy.position - transform.position).magnitude;
            if (currDistance > distance && currDistance < DetectionRange)
            {
                target = enemy.position;
            }
        }
        return target;
    }
    #endregion
}
