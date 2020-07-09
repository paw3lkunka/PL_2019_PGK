using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using TMPro;

public class WorldMapCursor : MonoBehaviour
{
    public float scaleFactor = 0.1f;

    public TextMeshProUGUI waterUsage;
    public TextMeshProUGUI faithUsage;

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
                WorldSceneManager.Instance.Cursor.transform.position = pos;
            }
        }

        SGUtils.DrawNavLine
        (
            lineRenderer,
            WorldSceneManager.Instance.Leader.transform.position,
            WorldSceneManager.Instance.Cursor.transform.position,
            out float pathLength
        );

        WorldSceneManager.Instance.ResUseIndicator.Water = pathLength * GameplayManager.Instance.waterUsageFactor;
        WorldSceneManager.Instance.ResUseIndicator.Faith = pathLength * GameplayManager.Instance.faithUsageFactor;
        WorldSceneManager.Instance.ResUseIndicator.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    void CursorPositionChanged(InputAction.CallbackContext context)
    {
    }

}
