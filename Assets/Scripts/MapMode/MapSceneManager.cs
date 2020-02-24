using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapSceneManager : MonoBehaviour
{
    #region Variables

    public static MapSceneManager Instance { get; private set; }

    public PlayerPositionController playerPositionController;

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
    }

    private void Update()
    {
        if (enabled)
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
        var nextCursorPosition = transform.position + new Vector3(joystickAxis.x, joystickAxis.y) * cursorRange;
        nextCursorPosition.z = 0;

        cursorInstance.position = nextCursorPosition;
    }

    private void MoveCursorJoystick()
    {
        var joystickAxis = Joystick.current.stick.ReadValue();
        var nextCursorPosition = transform.position + new Vector3(joystickAxis.x, joystickAxis.y) * cursorRange;
        nextCursorPosition.z = 0;

        cursorInstance.position = nextCursorPosition;
    }

    #endregion
}
