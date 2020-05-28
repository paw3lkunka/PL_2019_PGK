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

    [field: SerializeField, GUIName("CanBeFanatic"), GUIReadOnly]
    public bool CanBeFanatic { get; protected set; }

    [field: SerializeField, GUIName("IsFanatic"), GUIReadOnly]
    public bool IsFanatic { get; protected set; }

    [Range(0.0f, 0.01f)]
    public float switchFanaticChance = 0.0015f;

    public MonoBehaviour normalBehaviour;
    public MonoBehaviour fanaticBehaviour;

    private void AttackCursorPosition(InputAction.CallbackContext ctx)
    {
        Vector3 target = CombatCursorManager.Instance.MainCursor.transform.position;
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

    private void AttackNearbyEnemy(InputAction.CallbackContext ctx)
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

            ApplicationManager.Instance.Input.CombatMode.SetShootTarget.performed -= AttackCursorPosition;
            ApplicationManager.Instance.Input.CombatMode.SetShootTarget.performed += AttackNearbyEnemy;
        }
    }

    private void SetNormalState()
    {
        if (IsFanatic)
        {
            IsFanatic = false;
            normalBehaviour.enabled = true;
            fanaticBehaviour.enabled = false;

            ApplicationManager.Instance.Input.CombatMode.SetShootTarget.performed += AttackCursorPosition;
            ApplicationManager.Instance.Input.CombatMode.SetShootTarget.performed -= AttackNearbyEnemy;
        }
    }

    #region Event handlers

    private void FailBit()
    {
        attack?.HoldFire();
        SetNormalState();
    }

    private void FanatismStart() => CanBeFanatic = true;
    private void FanatismEnd() => CanBeFanatic = false;

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
        CombatSceneManager.Instance.ourCrew.Add(damageable);
        AudioTimeline.Instance.OnBeatFail += FailBit;
        GameplayManager.Instance.FanaticStart += FanatismStart;
        GameplayManager.Instance.FanaticEnd += FanatismEnd;
        IsFanatic = true; //HACK to ensure, that SetNormalState will work.
        SetNormalState();
    }

    private void OnDisable()
    {
        CombatSceneManager.Instance.ourCrew.Remove(damageable);
        AudioTimeline.Instance.OnBeatFail -= FailBit;
        GameplayManager.Instance.FanaticStart -= FanatismStart;
        GameplayManager.Instance.FanaticEnd -= FanatismEnd;
        ApplicationManager.Instance.Input.CombatMode.SetShootTarget.performed -= AttackCursorPosition;
        ApplicationManager.Instance.Input.CombatMode.SetShootTarget.performed -= AttackNearbyEnemy;
    }

    #endregion
}
