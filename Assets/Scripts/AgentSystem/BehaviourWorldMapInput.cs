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
    private Vector3 target;

    private LineRenderer lineRenderer;

    private void GoToCursorPosition(InputAction.CallbackContext ctx)
    {
        moveable.Go(target = WorldSceneManager.Instance.Cursor.transform.position);
        if (target != transform.position)
        {
            lineRenderer.gameObject.SetActive(true);
        }
    }

    #region MonoBehaviour

    private void Awake()
    {
        moveable = GetComponent<Moveable>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    private void Start()
    {
        ApplicationManager.Instance.Input.Gameplay.PrimaryAction.performed += GoToCursorPosition;
        ApplicationManager.Instance.Input.Gameplay.PrimaryAction.Enable();
        target = transform.position;
        lineRenderer.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (lineRenderer.gameObject.activeInHierarchy)
        {
            SGUtils.DrawNavLine(lineRenderer, transform.position, target, out _);
        }
    }
    
    private void OnDisable()
    {
        if (ApplicationManager.Instance)
        {
            ApplicationManager.Instance.Input.Gameplay.PrimaryAction.performed -= GoToCursorPosition;
            ApplicationManager.Instance.Input.Gameplay.PrimaryAction.Disable();
        }
    }

    #endregion
}
