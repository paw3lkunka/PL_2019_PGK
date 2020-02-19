﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class CrewSceneManager : MonoBehaviour
{
    #region Variables

    public static CrewSceneManager Instance { get; private set; }

    public List<GameObject> enemies = new List<GameObject>();

    public GameObject walkTargetIndicator;
    public GameObject shootTargetIndicator;

    public Vector2 startPoint;

    public bool combatMode = true;

    public NewInput input;

    public Transform cursorPrefab;
    [HideInInspector]
    public Transform cursorInstance;
    private SpriteRenderer cursorInstanceRenderer;

    [HideInInspector]
    public Transform cultLeader;
    public float cursorRange = 5.0f;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        input = GameManager.Instance.input;
        Instance = this;
    }

    private void OnEnable()
    {
        InitializeCursor();

        if(input != null)
        {
            input.Gameplay.SetWalkTarget.performed += SetWalkTargetIndicator;
            input.Gameplay.SetWalkTarget.Enable();

            if (combatMode)
            {
                input.CombatMode.SetShootTarget.performed += SetShootTargetIndicator;
                input.CombatMode.SetShootTarget.Enable();
            }
        }
    }
    
    private void OnDisable()
    {
        if(input != null)
        {
            input.Gameplay.SetWalkTarget.performed -= SetWalkTargetIndicator;
            input.Gameplay.SetWalkTarget.Disable();

            if (combatMode)
            {
                input.CombatMode.SetShootTarget.performed -= SetShootTargetIndicator;
                input.CombatMode.SetShootTarget.Disable();
            }
        }
    }

    private void Update()
    {
        Debug.Log(enemies.Count);
        if(enabled)
        {
            switch (GameManager.Instance.inputSchedule)
            {
                case InputSchedule.MouseKeyboard:
                    MoveCursorPointer();
                    break;

                case InputSchedule.Gamepad:
                    MoveCursorGamepad();
                    break;

                case InputSchedule.JoystickKeyboard:
                    MoveCursorJoystick();
                    break;

                case InputSchedule.Touchscreen:
                    break;
            }
        }

        var cursorLeaderDistance = Vector2.Distance(cultLeader.transform.position, cursorInstance.transform.position);

        var currentCursorColor = cursorInstanceRenderer.color;
        currentCursorColor.a = cursorLeaderDistance / cursorRange;
        cursorInstanceRenderer.color = currentCursorColor;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startPoint, .2f);
    }

    #endregion

    #region Component

    private void InitializeCursor()
    {
        Cursor.visible = false;
        cursorInstance = Instantiate(cursorPrefab, new Vector3(startPoint.x, startPoint.y), Quaternion.identity);

        cursorInstanceRenderer = cursorInstance.GetComponent<SpriteRenderer>();
        cursorInstanceRenderer.color = new Color(62.0f / 255.0f, 87.0f / 255.0f, 64.0f / 255.0f, 1.0f);
        cursorInstanceRenderer.sortingOrder = 10;
    }

    #endregion

    #region Input

    private void MoveCursorPointer()
    {
        var inputValue = Mouse.current.position.ReadValue();
        var nextCursorPosition = Camera.main.ScreenToWorldPoint(inputValue);
        nextCursorPosition.z = 0;

        cursorInstance.position = nextCursorPosition;
    }

    private void MoveCursorGamepad()
    {
        var joystickAxis = Gamepad.current.leftStick.ReadValue();
        var nextCursorPosition = cultLeader.position + new Vector3(joystickAxis.x, joystickAxis.y) * cursorRange;
        nextCursorPosition.z = 0;

        cursorInstance.position = nextCursorPosition;
    }

    private void MoveCursorJoystick()
    {
        var joystickAxis = Joystick.current.stick.ReadValue();
        var nextCursorPosition = cultLeader.position + new Vector3(joystickAxis.x, joystickAxis.y) * cursorRange;
        nextCursorPosition.z = 0;

        cursorInstance.position = nextCursorPosition;
    }

    private void SetWalkTargetIndicator(InputAction.CallbackContext ctx)
    {
        if (!GameManager.Gui.isMouseOver)
        {
            walkTargetIndicator.transform.position = cursorInstance.position;
        }
    }

    private void SetShootTargetIndicator(InputAction.CallbackContext ctx)
    {
        shootTargetIndicator.transform.position = cursorInstance.position;
    }

    #endregion
}

public static class Extensions
{
    public static (GameObject, float) NearestFrom(this List<GameObject> objs, Vector2 from)
    {
        GameObject target = null;
        float minimum = float.PositiveInfinity;

        foreach (GameObject enemy in objs)
        {
            if (!enemy)
            {
                continue;
            }

            float distance = Vector2.Distance(from, enemy.transform.position);
            if (distance < minimum)
            {
                target = enemy;
                minimum = distance;
            }
        }

        return (target, minimum);
    }
}
