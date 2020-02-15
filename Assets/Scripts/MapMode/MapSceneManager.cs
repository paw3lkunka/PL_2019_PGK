using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapSceneManager : MonoBehaviour
{
    #region Variables

    public static MapSceneManager Instance { get; private set; }

    private float size = 0.3f;
    private float delta = 0.5f;

    public Sprite Dot;

    private List<Vector2> positions = new List<Vector2>();
    private List<GameObject> dots = new List<GameObject>();

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

        if (input != null)
        {
            
        }
    }

    private void OnDisable()
    {
        if (input != null)
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
        }
    }

    private void Update()
    {
        Vector3 vel = new Vector3();
        cursorInstance.position = Vector3.SmoothDamp(cursorInstance.position, nextCursorPosition, ref vel, 0.05f);
    }

    private void FixedUpdate()
    {
        if (positions.Count > 0)
        {
            DestroyAllDots();
            positions.Clear();
        }
    }

    #endregion

    #region Component

    private void InitializeCursor()
    {
        Cursor.visible = false;
        cursorInstance = Instantiate(cursorPrefab, GameManager.Instance.savedPosition, Quaternion.identity);

        cursorInstanceRenderer = cursorInstance.GetComponent<SpriteRenderer>();
        cursorInstanceRenderer.color = new Color(62.0f / 255.0f, 87.0f / 255.0f, 64.0f / 255.0f, 1.0f);
        cursorInstanceRenderer.sortingOrder = 10;
    }

    public void DrawDottedLine(Vector2 start, Vector2 end)
    {
        DestroyAllDots();

        Vector2 point = start;
        Vector2 direction = (end - start).normalized;

        while ((end - start).magnitude > (point - start).magnitude)
        {
            positions.Add(point);
            point += (direction * transform.localScale.x * delta);
        }

        Render();
    }

    private void Render()
    {
        foreach (var position in positions)
        {
            var g = GetOneDot();
            g.transform.position = position;
            dots.Add(g);
        }
    }

    private GameObject GetOneDot()
    {
        var gameObject = new GameObject();
        gameObject.transform.localScale = transform.localScale * size;
        gameObject.transform.parent = transform;

        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Dot;
        return gameObject;
    }

    private void DestroyAllDots()
    {
        foreach (var dot in dots)
        {
            Destroy(dot);
        }
        dots.Clear();
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
        var joystickAxis = ctx.ReadValue<Vector2>() * cursorRange;
        nextCursorPosition = cultLeader.position + new Vector3(joystickAxis.x, joystickAxis.y);
        nextCursorPosition.z = 0;
    }

    #endregion
}
