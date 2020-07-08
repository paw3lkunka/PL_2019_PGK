using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class WorldMapCursor : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        ApplicationManager.Instance.Input.Gameplay.MoveCursor.performed += CursorPositionChanged;
        ApplicationManager.Instance.Input.Gameplay.MoveCursor.Enable();
    }

    private void OnDisable()
    {
        ApplicationManager.Instance.Input.Gameplay.MoveCursor.performed -= CursorPositionChanged;
        ApplicationManager.Instance.Input.Gameplay.MoveCursor.Disable();
    }

    private void Update()
    {
        if (ApplicationManager.Instance.CurrentInputScheme == InputSchemeEnum.MouseKeyboard)
        {
            Vector3 pos = default; 
            if (SGUtils.CameraToGrounRaycast(Camera.main, 1000, ref pos))
            {
                WorldSceneManager.Instance.cursor.transform.position = pos;
            }
        }

        SGUtils.DrawNavLine
        (
            lineRenderer,
            WorldSceneManager.Instance.leader.transform.position,
            WorldSceneManager.Instance.cursor.transform.position
        );
    }

    void CursorPositionChanged(InputAction.CallbackContext context)
    {
        Debug.Log("Action: " + context.action.id);
    }

}
