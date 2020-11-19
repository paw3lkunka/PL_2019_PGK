﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Damageable),typeof(Detection))]
public class Cultist : MonoBehaviour
{
    private Damageable damageable;
    private Detection detection;
    private IAttack attack;
    public CultistEntityInfo info;

    [field: SerializeField, GUIName("CanBeFanatic"), GUIReadOnly]
    public bool CanBeFanatic { get; protected set; }

    [field: SerializeField, GUIName("IsFanatic"), GUIReadOnly]
    public bool IsFanatic { get; protected set; }

    [Range(0.0f, 0.01f)]
    public float switchFanaticChance = 0.0015f;

    public MonoBehaviour normalBehaviour;
    public MonoBehaviour fanaticBehaviour;

    private void AttackInDirection(bool _)
    {
        detection.detectionDirection = CombatCursorManager.Instance.shootDirection;

        if (RhythmMechanics.Instance.Combo > 0)
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

    private void ToggleState()
    {
        if (IsFanatic)
        {
            SetNormalState();
        }
        else
        {
            SetFanaticState();
        }
    }

    private void SetFanaticState()
    {
        if (!IsFanatic)
        {
            IsFanatic = true;
            normalBehaviour.enabled = false;
            fanaticBehaviour.enabled = true;
            DetectInFullCircle = true;

            AudioTimeline.Instance.OnBeat -= AttackInDirection;
            AudioTimeline.Instance.OnBeat += AttackNearbyEnemy;
        }
    }

    private void SetNormalState()
    {
        if (IsFanatic)
        {
            IsFanatic = false;
            normalBehaviour.enabled = true;
            fanaticBehaviour.enabled = false;
            DetectInFullCircle = false;

            AudioTimeline.Instance.OnBeat += AttackInDirection;
            AudioTimeline.Instance.OnBeat -= AttackNearbyEnemy;
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
        Debug.Log("FailBit");
        attack?.HoldFire();
        SetNormalState();
    }

    private void FanatismStart() => CanBeFanatic = true;
    private void FanatismEnd() => CanBeFanatic = false;

    private void OnDamage(float damage)
    {
        GameplayManager.Instance.DecreseFaithByCultistWounded();
        //TODO - some indication?
    }

    private void OnDeath()
    {
        GameplayManager.Instance.cultistInfos.Remove(info);
        GameplayManager.Instance.Health.Max -= damageable.Health.Max;
    }

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        damageable = GetComponent<Damageable>();
        detection = GetComponent<Detection>();
        attack = GetComponent<IAttack>();
    }

    private void Update()
    {
        if (CanBeFanatic && Random.Range(0.0f, 1.0f) < switchFanaticChance)
        {
            ToggleState();
        }
    }

    private void OnEnable()
    {
        LocationManager.Instance.ourCrew.Add(damageable);
        if (AudioTimeline.Instance)
        {
            AudioTimeline.Instance.OnBeatFail += FailBit;
        }
        else
        {
            Debug.LogWarning("No audioTimeline!");
        }
        GameplayManager.Instance.FanaticStart += FanatismStart;
        GameplayManager.Instance.FanaticEnd += FanatismEnd;

        damageable.DamageTaken += OnDamage;
        damageable.Death += OnDeath;
        IsFanatic = true; //HACK to ensure, that SetNormalState will work.
        SetNormalState();
    }

    private void OnDisable()
    {
        LocationManager.Instance?.ourCrew.Remove(damageable);
        if (AudioTimeline.Instance)
        {
            AudioTimeline.Instance.OnBeatFail -= FailBit;
        }
        GameplayManager.Instance.FanaticStart -= FanatismStart;
        GameplayManager.Instance.FanaticEnd -= FanatismEnd;
        AudioTimeline.Instance.OnBeat -= AttackInDirection;
        AudioTimeline.Instance.OnBeat -= AttackNearbyEnemy;
        damageable.DamageTaken -= OnDamage;
        damageable.Death -= OnDeath;
    }

    private void OnDestroy()
    {
        info?.Save(this);
    }

    #endregion
}
