using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BehaviourWorldMapInput : MonoBehaviour
{
    private Moveable moveable;
    //todo position in formation
    public Vector3 formationOffset = Vector3.zero;

    private void GoToCursorPosition(InputAction.CallbackContext ctx)
    {
        var inputValue = Mouse.current.position.ReadValue();
        var ray = Camera.main.ScreenPointToRay(inputValue);

        foreach (var hit in Physics.RaycastAll(ray))
        {
            // TODO: replace tag with layer mask
            if (hit.collider.CompareTag("Ground"))
            {
                moveable.Go(hit.point);
                continue;
            }
        }
    }


    #region MonoBehaviour

    private void Awake()
    {
        moveable = GetComponent<Moveable>();
    }

    private void OnEnable()
    {
        ApplicationManager.Instance.Input.Gameplay.SetWalkTarget.performed += GoToCursorPosition;
        ApplicationManager.Instance.Input.Gameplay.SetWalkTarget.Enable();
    }

    private void OnDisable()
    {
        if (ApplicationManager.Instance)
        {
            ApplicationManager.Instance.Input.Gameplay.SetWalkTarget.performed -= GoToCursorPosition;
            ApplicationManager.Instance.Input.Gameplay.SetWalkTarget.Disable();
        }
    }

    #endregion
}
