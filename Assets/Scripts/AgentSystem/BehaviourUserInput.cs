using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Moveable))]
public class BehaviourUserInput : MonoBehaviour, IBehaviour
{
    public static bool controlEnabled = true;
    private Moveable moveable;
    private Vector3 cursor;
    //todo position in formation
    public Vector3 formationOffset = Vector3.zero;

    public void UpdateTarget(Vector3? target)
    {
        moveable.Go(target ?? cursor);
    }

    private void GoToCursorPosition()
    {
        if (controlEnabled)
        {
            cursor = CombatCursorManager.Instance.walkTargetIndicator.transform.position + formationOffset;
            UpdateTarget(cursor);
        }
    }


    #region MonoBehaviour

    private void Awake()
    {
        moveable = GetComponent<Moveable>();
    }

    private void OnEnable()
    {
        CombatCursorManager.Instance.OnSetWalkTarget += GoToCursorPosition;
    }

    private void OnDisable()
    {
        CombatCursorManager.Instance.OnSetWalkTarget -= GoToCursorPosition;
    }

    #endregion
}
