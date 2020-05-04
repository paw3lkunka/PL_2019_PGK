using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cultist : MonoBehaviour
{
    private Damageable damageable;
    private IAttack attack;
    private IBehaviour behaviour;



    #region MonoBehaviour

    private void Awake()
    {
        damageable = GetComponent<Damageable>();
        attack = GetComponent<IAttack>();
        behaviour = GetComponent<IBehaviour>();
    }

    private void AttackCursorPosition(InputAction.CallbackContext ctx)
    {
        Vector3 target = CombatCursorManager.Instance.MainCursor.transform.position;
        
        if (RhythmMechanics.Instance.Combo > 0)
        {
            attack?.Attack(target);
        }
    }

    private void FailBit() => attack?.HoldFire();

    private void OnEnable()
    {
        CombatSceneManager.Instance.ourCrew.Add(damageable);
        ApplicationManager.Instance.Input.CombatMode.SetShootTarget.performed += AttackCursorPosition;
        AudioTimeline.Instance.OnBeatFail += FailBit;
    }

    private void OnDisable()
    {
        CombatSceneManager.Instance.ourCrew.Remove(damageable);
        ApplicationManager.Instance.Input.CombatMode.SetShootTarget.performed -= AttackCursorPosition;
        AudioTimeline.Instance.OnBeatFail -= FailBit;
    }

    #endregion
}
