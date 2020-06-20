using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Detection : MonoBehaviour
{
    //TODO replace with cultist list reference
    private List<Damageable> enemies;


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
        strongest,
        /// <summary>
        /// Returns target with smallest amount of heaktg in detection range.
        /// </summary>
        weakest,
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
                case DetectionStrategy.random:
                    func = RandomStrategy;
                    break;
                case DetectionStrategy.nearest:
                    func = NearestStartegy;
                    break;
                case DetectionStrategy.farthest:
                    func = FarthestStartegy;
                    break;
                case DetectionStrategy.strongest:
                    func = StrongestStartegy;
                    break;
                case DetectionStrategy.weakest:
                    func = WeakestStartegy;
                    break;
                default:
                    func = UndefinedStrategy;
                    break;
            }
            return func;
        }
    }

    #region Strategies
    private Vector3? AccidentalStartegy()
    {
        foreach (var enemy in enemies)
        {
            if ((enemy.transform.position - transform.transform.position).magnitude < DetectionRange)
            {
                return enemy.transform.position;
            }
        }
        return null;
    }

    private Vector3? RandomStrategy()
    {
        int length = enemies.Count;
        int[] order = new int[length];

        for (int i = 0; i < length; i++)
        {
            order[i] = i;
        }

        for (int i = 0; i < length; i++)
        {
            int rand = UnityEngine.Random.Range(0, length);
            (order[i], order[rand]) = (order[rand], order[i]);
        }

        foreach (int i in order)
        {
            if ((enemies[i].transform.position - transform.transform.position).magnitude < DetectionRange)
            {
                return enemies[i].transform.position;
            }
        }
        return null;
    }

    private Vector3? NearestStartegy()
    {
        float distance = DetectionRange;
        Vector3? target = null;
        foreach (var enemy in enemies)
        {
            float currDistance = (enemy.transform.position - transform.position).magnitude;

            if (currDistance < distance)
            {
                distance = currDistance;
                target = enemy.transform.position;
            }
        }
        return target;
    }

    private Vector3? FarthestStartegy()
    {
        float distance = 0;
        Vector3? target = null;
        foreach (var enemy in enemies)
        {
            float currDistance = (enemy.transform.position - transform.position).magnitude;

            if (currDistance > distance && currDistance < DetectionRange)
            {
                target = enemy.transform.position;
            }
        }
        return target;
    }

    private Vector3? StrongestStartegy()
    {
        float hp = 0;
        Vector3? target = null;
        foreach (var enemy in enemies)
        {
            float currDistance = (enemy.transform.position - transform.position).magnitude;

            if (currDistance < DetectionRange && enemy.Health > hp)
            {
                hp = enemy.Health;
                target = enemy.transform.position;
            }
        }
        return target;
    }

    private Vector3? WeakestStartegy()
    {
        float hp = float.PositiveInfinity;
        Vector3? target = null;
        foreach (var enemy in enemies)
        {
            float currDistance = (enemy.transform.position - transform.position).magnitude;

            if (currDistance < DetectionRange && enemy.Health < hp)
            {
                hp = enemy.Health;
                target = enemy.transform.position;
            }
        }
        return target;
    }

    private Vector3? UndefinedStrategy()
    {
        Debug.LogError($"{gameObject.name} use undefined strategy!");
        return null;
    }
    #endregion

    #region MonoBehaviour
    private void Start()
    {
        if (gameObject.layer == LayerMask.NameToLayer("PlayerCrew"))
        {
            enemies = LocationManager.Instance.enemies;
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            enemies = LocationManager.Instance.ourCrew;
        }
        else
        {
            Debug.LogError($"GameObject {gameObject.name} has invalid layer!");
        }
    }

    #endregion

    #region In editor

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, DetectionRange);
    }

    #endregion

}
