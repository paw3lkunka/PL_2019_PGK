using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Damageable),typeof(Detection))]
public class Cultist : MonoBehaviour
{
    private Damageable damageable;
    private Detection detection;
    private IAttack attack;
    private IBoostable[] boostables;
    public CultistEntityInfo info;

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
                if (detection.Func() == null)
                {
                    attack?.HoldFire();
                }
            }
        }
    }

    private bool DetectInFullCircle
    {
        get => detection.detectionHalfAngle >= 180.0f;
        set => detection.detectionHalfAngle = value ? 181.0f : CombatCursorManager.Instance.shootHalfAngle;
    }

    #region Event handlers

    private void FailBit()
    {
        attack?.HoldFire();
    }

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
        //DetectInFullCircle = true;

        //AudioTimeline.Instance.OnBeat -= AttackInDirection;
        //AudioTimeline.Instance.OnBeat += AttackNearbyEnemy;

        foreach (var item in boostables)
        {
            item.BState = BoostableState.boosted;
        }
    }

    private void OnOverfaithEnd()
    {
        //normalBehaviour.enabled = true;
        //overfaithBehaviour.enabled = false;
        //DetectInFullCircle = false;

        //AudioTimeline.Instance.OnBeat += AttackInDirection;
        //AudioTimeline.Instance.OnBeat -= AttackNearbyEnemy;

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
        attack = GetComponent<IAttack>();
        boostables = GetComponentsInChildren<IBoostable>();

        //AudioTimeline.Instance.OnBeat += AttackInDirection;
    }

    private void Update()
    {

    }

    private void OnEnable()
    {
        LocationManager.Instance.ourCrew.Add(damageable);
        if (AudioTimeline.Instance)
        {
            AudioTimeline.Instance.OnBeatFail += FailBit;
            AudioTimeline.Instance.OnBeat += AttackInDirection;
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
