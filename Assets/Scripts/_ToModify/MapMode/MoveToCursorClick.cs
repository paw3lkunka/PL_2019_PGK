﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveToCursorClick : MonoBehaviour
{
    #region Variables

    public float speed = 1.0f;
    private Vector2 targetPos;

    private NewInput input;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        input = ApplicationManager.Instance.Input;
    }

    private void Start()
    {
        targetPos = GameplayManager.Instance.lastWorldMapPosition;
    }

    private void OnEnable()
    {
        if (input != null)
        {
            input.Gameplay.SetWalkTarget.performed += SetWalkTarget;
            input.Gameplay.SetWalkTarget.Enable();
        }
    }

    private void OnDisable()
    {
        if (input != null)
        {
            input.Gameplay.SetWalkTarget.performed -= SetWalkTarget;
            input.Gameplay.SetWalkTarget.Disable();
        }
    }

    private void LateUpdate()
    {
        if ((Vector2)transform.position != targetPos && GameplayManager.Instance.ourCrew.Count > 0)
        {
            MapSceneManager.Instance.DrawDottedLine(targetPos, transform.position);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
    }

    #endregion

    #region Component
    
    #endregion

    #region Input

    private void SetWalkTarget(InputAction.CallbackContext ctx)
    {
        targetPos = MapSceneManager.Instance.cursorInstance.position;
    }

    #endregion
}