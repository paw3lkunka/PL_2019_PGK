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
        random,
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

    public Vector3 detectionDirection = Vector3.right;

    public float detectionHalfAngle = 181.0f;

    public bool includeRaycastTest = true;

    private int raycastLayerMask;

#if UNITY_EDITOR
    [Tooltip("Show range sphere gizmo in editor")]
    public bool showRangeBounds = false;
    public bool showAngleBounds = false;
#endif

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
            if (IsDetected(enemy, out _))
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
            if (IsDetected(enemies[i], out _))
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
            if (IsDetected(enemy, out float currDistance) && distance > currDistance)
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
            if (IsDetected(enemy, out float currDistance) && distance < currDistance)
            {
                distance = currDistance;
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
            if (IsDetected(enemy, out _) && enemy.Health > hp)
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
            if (IsDetected(enemy, out _) && enemy.Health < hp)
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


    private bool IsDetected(Component enemy, out float distance)
    {
        if(enemy != null)
        {
            var vectorToEnemy = enemy.transform.position - transform.position;

            distance = vectorToEnemy.magnitude;
            var vectorToEnemyNormalized = vectorToEnemy.normalized;
            var angleToEnemy = Vector3.Angle(vectorToEnemyNormalized, detectionDirection);

            if (distance < DetectionRange && angleToEnemy < detectionHalfAngle)
            {
                if (includeRaycastTest)
                {
                    Physics.Raycast(transform.position, vectorToEnemyNormalized, out RaycastHit hitInfo, distance + 1.0f, raycastLayerMask);
                    return hitInfo.collider.gameObject == enemy.gameObject;
                }
                else
                {
                    return true;
                }
            }
        }
        distance = float.PositiveInfinity;
        return false;
    }

    #region MonoBehaviour
    private void Start()
    {
        if (gameObject.layer == LayerMask.NameToLayer("PlayerCrew"))
        {
            enemies = LocationManager.Instance.enemies;
            raycastLayerMask = LayerMask.GetMask("Obstacles", "GroundObstacles", "Enemies");
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            enemies = LocationManager.Instance.ourCrew;
            raycastLayerMask = LayerMask.GetMask("Obstacles", "GroundObstacles", "PlayerCrew");
        }
        else
        {
            Debug.LogError($"GameObject {gameObject.name} has invalid layer!");
        }
    }

    #endregion

    #region In editor

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (showRangeBounds)
        {
            Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.3f);
            Gizmos.DrawSphere(transform.position, DetectionRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, DetectionRange);
        }
        if (showAngleBounds)
        {
            Vector3 center = transform.position;
            Vector3 shootDirection = CombatCursorManager.Instance.shootDirection;
            Vector3 forward = transform.position + shootDirection * DetectionRange;
            Vector3 corner1 = transform.position + Quaternion.Euler(0, detectionHalfAngle, 0) * shootDirection * DetectionRange;
            Vector3 corner2 = transform.position + Quaternion.Euler(0, -detectionHalfAngle, 0) * shootDirection * DetectionRange;

            Vector3 mid11 = transform.position + Quaternion.Euler(0, detectionHalfAngle * 1.0f / 5.0f, 0) * shootDirection * DetectionRange;
            Vector3 mid12 = transform.position + Quaternion.Euler(0, detectionHalfAngle * 2.0f / 5.0f, 0) * shootDirection * DetectionRange;
            Vector3 mid13 = transform.position + Quaternion.Euler(0, detectionHalfAngle * 3.0f / 5.0f, 0) * shootDirection * DetectionRange;
            Vector3 mid14 = transform.position + Quaternion.Euler(0, detectionHalfAngle * 4.0f / 5.0f, 0) * shootDirection * DetectionRange;

            Vector3 mid21 = transform.position + Quaternion.Euler(0, -detectionHalfAngle * 1.0f / 5.0f, 0) * shootDirection * DetectionRange;
            Vector3 mid22 = transform.position + Quaternion.Euler(0, -detectionHalfAngle * 2.0f / 5.0f, 0) * shootDirection * DetectionRange;
            Vector3 mid23 = transform.position + Quaternion.Euler(0, -detectionHalfAngle * 3.0f / 5.0f, 0) * shootDirection * DetectionRange;
            Vector3 mid24 = transform.position + Quaternion.Euler(0, -detectionHalfAngle * 4.0f / 5.0f, 0) * shootDirection * DetectionRange;

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(center, forward);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(center, corner1);
            Gizmos.DrawLine(center, corner2);

            Gizmos.DrawLine(forward, mid11);
            Gizmos.DrawLine(mid11, mid12);
            Gizmos.DrawLine(mid12, mid13);
            Gizmos.DrawLine(mid13, mid14);
            Gizmos.DrawLine(mid14, corner1);

            Gizmos.DrawLine(forward, mid21);
            Gizmos.DrawLine(mid21, mid22);
            Gizmos.DrawLine(mid22, mid23);
            Gizmos.DrawLine(mid23, mid24);
            Gizmos.DrawLine(mid24, corner2);

        }
    }
#endif

#endregion

}
