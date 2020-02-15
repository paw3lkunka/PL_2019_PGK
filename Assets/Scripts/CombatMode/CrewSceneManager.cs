using System.Collections;
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
    private Vector3 nextCursorPosition;
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
            switch (GameManager.Instance.inputSchedule)
            {
                case InputSchedule.MouseKeyboard:
                    input.Gameplay.MoveCursor.performed += MoveCursorPointer;
                    break;

                case InputSchedule.Gamepad:
                    input.Gameplay.MoveCursor.performed += MoveCursorJoystick;
                    input.Gameplay.MoveCursor.canceled += ctx => nextCursorPosition = cultLeader.position;
                    break;

                case InputSchedule.Touchscreen:
                    break;
            }
            input.Gameplay.MoveCursor.Enable();

            input.Gameplay.SetWalkTarget.performed += SetShootTargetIndicator;
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
            switch (GameManager.Instance.inputSchedule)
            {
                case InputSchedule.MouseKeyboard:
                    input.Gameplay.MoveCursor.performed -= MoveCursorPointer;
                    break;

                case InputSchedule.Gamepad:
                    input.Gameplay.MoveCursor.performed -= MoveCursorJoystick;
                    input.Gameplay.MoveCursor.canceled -= ctx => cursorInstance.position = cultLeader.position;
                    break;

                case InputSchedule.Touchscreen:
                    break;
            }
            input.Gameplay.MoveCursor.Disable();

            input.Gameplay.SetWalkTarget.performed -= SetShootTargetIndicator;
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
        Vector3 vel = new Vector3();
        cursorInstance.position = Vector3.SmoothDamp(cursorInstance.position, nextCursorPosition, ref vel, 0.05f);
        
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

    public void MoveCursorPointer(InputAction.CallbackContext ctx)
    {
        nextCursorPosition = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>());
        nextCursorPosition.z = 0;
    }

    public void MoveCursorJoystick(InputAction.CallbackContext ctx)
    {
        var joystickAxis = ctx.ReadValue<Vector2>();
        nextCursorPosition = cultLeader.position + new Vector3(joystickAxis.x, joystickAxis.y) * cursorRange;
        nextCursorPosition.z = 0;
    }

    public void SetWalkTargetIndicator(InputAction.CallbackContext ctx)
    {
        walkTargetIndicator.transform.position = cursorInstance.position;
    }

    public void SetShootTargetIndicator(InputAction.CallbackContext ctx)
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
