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

    public float cursorRange = 20.0f;
    public Transform cultLeaderCamera;

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

    private void FixedUpdate()
    {
        if(!GameplayManager.Instance.IsPaused)
        {
            switch (ApplicationManager.Instance.CurrentInputScheme)
            {
                case InputSchemeEnum.MouseKeyboard:
                    MoveCursorPointer();
                    break;

                case InputSchemeEnum.Gamepad:
                    MoveCursorGamepad();
                    break;

                case InputSchemeEnum.JoystickKeyboard:
                    MoveCursorJoystick();
                    break;

                case InputSchemeEnum.Touchscreen:
                    break;
            }

            SGUtils.DrawNavLine
            (
                lineRenderer,
                WorldSceneManager.Instance.Leader.transform.position,
                WorldSceneManager.Instance.Cursor.transform.position,
                out float pathLength
            );

            float speed = WorldSceneManager.Instance.Leader.GetComponent<NavMeshAgent>().speed;
            WorldSceneManager.Instance.ResUseIndicator.Water = CalculateUsage(speed, pathLength, WorldSceneManager.Instance.ResourceDepleter.WaterDepletionRate);

            WorldSceneManager.Instance.ResUseIndicator.Faith = CalculateUsage(speed, pathLength, WorldSceneManager.Instance.ResourceDepleter.FaithDepletionRate);
            WorldSceneManager.Instance.ResUseIndicator.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        }
    }

    void CursorPositionChanged(InputAction.CallbackContext context)
    {
    }

    private float CalculateUsage(float speed, float route, float usage)
    {
        float time = route / speed;
        return usage * (time / Time.fixedDeltaTime);
    }

    private void MoveCursorPointer()
    {
        Vector3 pos = default;
        if (SGUtils.CameraToGroundNearestRaycast(Camera.main, 1000, ref pos))
        {
            WorldSceneManager.Instance.Cursor.transform.position = pos;
        }
    }

    private void MoveCursorGamepad()
    {
        var joystickAxis = Gamepad.current.leftStick.ReadValue();
        SetCursorForJoyAxis(joystickAxis);
    }

    private void MoveCursorJoystick()
    {
        var joystickAxis = Joystick.current.stick.ReadValue();
        SetCursorForJoyAxis(joystickAxis);
    }

    private void SetCursorForJoyAxis(Vector2 joystickAxis)
    {
        var nextCursorPosition = WorldSceneManager.Instance.Leader.transform.position;
        var cursorOffset = new Vector3(-joystickAxis.y, 0.0f, joystickAxis.x) * cursorRange;

        if(cursorOffset.magnitude > 1.0f)
        {
            if(cultLeaderCamera)
            {
                cursorOffset = Quaternion.Euler(0.0f, cultLeaderCamera.localEulerAngles.y + 90.0f, 0.0f) * cursorOffset;
            }

            nextCursorPosition += cursorOffset;
        }

        WorldSceneManager.Instance.Cursor.transform.position = nextCursorPosition;
    }
}
