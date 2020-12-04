using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script in CultistLeader_WorldMap
/// </summary>
public class BehaviourWorldMapInput : MonoBehaviour
{
    private Moveable moveable;
    //todo position in formation
    private Vector3 target;

    public Vector3 formationOffset = Vector3.zero;

    private LineRenderer lineRenderer;

    private void GoToCursorPosition(InputAction.CallbackContext ctx)
    {
        moveable.Go(target = WorldSceneManager.Instance.Cursor.transform.position);
    }


    #region MonoBehaviour

    private void Awake()
    {
        moveable = GetComponent<Moveable>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    private void Start()
    {
        ApplicationManager.Instance.Input.Gameplay.SetWalkTarget.performed += GoToCursorPosition;
        ApplicationManager.Instance.Input.Gameplay.SetWalkTarget.Enable();
        target = transform.position;
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
