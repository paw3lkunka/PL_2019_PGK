using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Damageable),typeof(Detection),typeof(Moveable))]
[RequireComponent(typeof(IAttack),typeof(Detection),typeof(Moveable))]
public class Cultist : MonoBehaviour
{
    private Moveable moveable;
    private Damageable damageable;
    private Detection detection;
    private IAttack attack;
    private IBoostable[] boostables;
    public CultistEntityInfo info;

    public Vector2 attackMinMax;
    public Vector2 speedMinMax;

    public MonoBehaviour normalBehaviour;
    [UnityEngine.Serialization.FormerlySerializedAs("fanaticBehaviour")]
    public MonoBehaviour overfaithBehaviour;

    private void AttackInDirection(bool _)
    {
        detection.detectionDirection = CombatCursorManager.Instance.shootDirection;

        if (RhythmMechanics.Instance.Combo > 0 && CombatCursorManager.Instance.CanShoot)
        {
            Vector3? detected = detection.Func();

            if (detected.HasValue)
            {
                attack?.Attack(detected.Value);
            }
            else
            {
                attack?.HoldFire();
            }
        }
        else
        {
            attack?.HoldFire();
        }
    }

    private void AttackNearbyEnemy(bool _)
    {
        Vector3? detected = detection.Func();
        if (detected.HasValue)
        {
            Vector3 target = detected.Value;
            target.y = transform.position.y;
            if (RhythmMechanics.Instance.Combo > 0)
            {
                attack?.Attack(target);
            }
        }
        else if (detected == null)
        {
            attack?.HoldFire();
        }
    }

    private bool DetectInFullCircle
    {
        get => detection.detectionHalfAngle >= 180.0f;
        set => detection.detectionHalfAngle = value ? 181.0f : CombatCursorManager.Instance.shootHalfAngle;
    }

    #region Event handlers

    private void FailBit(bool reset)
    {
        attack?.HoldFire();
        if (GameplayManager.Instance.dontMoveOnFail)
        {
            moveable.flags = Moveable.Flags.nothing;
            moveable.Stop();
        }
    }

    private void OnBeatMove(bool _) => moveable.flags = Moveable.Flags.canMove;

    private void OnLowFaithStart()
    {
        foreach (var item in boostables)
        {
            item.BState = BoostableState.decresed;
        }
    }

    private void OnLowFaithEnd()
    {
        foreach (var item in boostables)
        {
            if (item.IsDecresed)
            {
                item.BState = BoostableState.normal;
            }
        }
    }

    private void OnOverfaithStart()
    {
        //normalBehaviour.enabled = false;
        //overfaithBehaviour.enabled = true;

        DetectInFullCircle = true;

        AudioTimeline.Instance.OnBeat -= AttackInDirection;
        AudioTimeline.Instance.OnBeat += AttackNearbyEnemy;

        foreach (var item in boostables)
        {
            item.BState = BoostableState.boosted;
        }
    }

    private void OnOverfaithEnd()
    {
        //normalBehaviour.enabled = true;
        //overfaithBehaviour.enabled = false;

        DetectInFullCircle = false;

        AudioTimeline.Instance.OnBeat += AttackInDirection;
        AudioTimeline.Instance.OnBeat -= AttackNearbyEnemy;

        foreach (var item in boostables)
        {
            if (item.IsBoosted)
            {
                item.BState = BoostableState.normal;
            }
        }
    }

    private void OnDamage(float damage)
    {
        GameplayManager.Instance.DecreseFaithByCultistWounded();
        //TODO - some indication?
    }

    private void OnDeath()
    {
        GameplayManager.Instance.cultistInfos.Remove(info);
    }

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        damageable = GetComponent<Damageable>();
        detection = GetComponent<Detection>();
        moveable = GetComponent<Moveable>();
        attack = GetComponent<IAttack>();
        boostables = GetComponentsInChildren<IBoostable>();
        moveable.flags = Moveable.Flags.nothing;
        //AudioTimeline.Instance.OnBeat += AttackInDirection;
    }

    private void Update()
    {
        float faith = GameplayManager.Instance.Faith.Normalized;

        moveable.SpeedBase = Mathf.Lerp(speedMinMax.x, speedMinMax.y, faith);
        attack.DamageBaseMultiplier = Mathf.Lerp(attackMinMax.x, attackMinMax.y, faith);
    }

    private void OnEnable()
    {
        LocationManager.Instance.ourCrew.Add(damageable);
        if (AudioTimeline.Instance)
        {
            AudioTimeline.Instance.OnBeatFail += FailBit;
            AudioTimeline.Instance.OnBeat += AttackInDirection;
            AudioTimeline.Instance.OnBeat += OnBeatMove;
        }
        else
        {
            Debug.LogWarning("No audioTimeline!");
        }

        GameplayManager.Instance.LowFaithLevelStart += OnLowFaithStart;
        GameplayManager.Instance.LowFaithLevelEnd += OnLowFaithEnd;
        GameplayManager.Instance.OverfaithStart += OnOverfaithStart;
        GameplayManager.Instance.OverfaithEnd += OnOverfaithEnd;

        damageable.DamageTaken += OnDamage;
        damageable.Death += OnDeath;
    }

    private void OnDisable()
    {
        LocationManager.Instance?.ourCrew.Remove(damageable);
        if (AudioTimeline.Instance)
        {
            AudioTimeline.Instance.OnBeatFail -= FailBit;
            AudioTimeline.Instance.OnBeat -= AttackInDirection;
            AudioTimeline.Instance.OnBeat -= OnBeatMove;
            //AudioTimeline.Instance.OnBeat -= AttackNearbyEnemy;
        }

        GameplayManager.Instance.LowFaithLevelStart -= OnLowFaithStart;
        GameplayManager.Instance.LowFaithLevelEnd -= OnLowFaithEnd;
        GameplayManager.Instance.OverfaithStart -= OnOverfaithStart;
        GameplayManager.Instance.OverfaithEnd -= OnOverfaithEnd;

        damageable.DamageTaken -= OnDamage;
        damageable.Death -= OnDeath;
    }

    private void OnDestroy()
    {
        info?.Save(this);
    }

    #endregion
}
