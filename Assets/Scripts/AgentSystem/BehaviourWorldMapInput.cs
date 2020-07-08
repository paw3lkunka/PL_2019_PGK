using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BehaviourWorldMapInput : MonoBehaviour
{
    private Moveable moveable;
    //todo position in formation
    private Vector3 target;

    public Vector3 formationOffset = Vector3.zero;

    private LineRenderer lineRenderer;

    private void GoToCursorPosition(InputAction.CallbackContext ctx)
    {
        Vector3 pos = default;
        if (SGUtils.CameraToGrounRaycast(Camera.main, 1000, ref pos))
        {
            moveable.Go(target = pos);
        }
    }


    #region MonoBehaviour

    private void Awake()
    {
        moveable = GetComponent<Moveable>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        ApplicationManager.Instance.Input.Gameplay.SetWalkTarget.performed += GoToCursorPosition;
        ApplicationManager.Instance.Input.Gameplay.SetWalkTarget.Enable();
    }

    private void Update()
    {
        SGUtils.DrawNavLine(lineRenderer, transform.position, target);
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
