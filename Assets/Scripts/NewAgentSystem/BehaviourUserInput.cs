using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Moveable))]
public class BehaviourUserInput : MonoBehaviour, IBehaviour
{
    private Moveable moveable;
    private Vector3 cursor;
    //todo position in formation
    public Vector3 offset = Vector3.zero;

    public void UpdateTarget(Vector3? target)
    {
        moveable.Go(target ?? cursor);
    }

    private void GoToCursorPosition(InputAction.CallbackContext ctx)
    {
        //TODO rythm sync
        cursor = CombatCursorManager.Instance.MainCursor.transform.position + offset;
        UpdateTarget(cursor);
    }


    #region MonoBehaviour

    private void Awake()
    {
        moveable = GetComponent<Moveable>();
    }

    private void OnEnable()
    {
        ApplicationManager.Instance.Input.Gameplay.SetWalkTarget.performed += GoToCursorPosition;
    }

    private void OnDisable()
    {
        ApplicationManager.Instance.Input.Gameplay.SetWalkTarget.performed -= GoToCursorPosition;
    }

    #endregion
}
