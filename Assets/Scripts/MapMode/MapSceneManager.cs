using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapSceneManager : MonoBehaviour, IInfoLogInvoker
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
        InfoLogInvokerExtensions.SetInfoLog(this);
    }

    private void Update()
    {
        if (enabled)
        {
            switch (GameManager.Instance.currentInputScheme)
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

    #region IInfoLogInvoker

    public void SetInfoLog()
    {
        var shrinesVisited = GameManager.Instance.ShrinesVisited.Count;
        string mapInfoLogHeader = null;
        string mapInfoLogText = null;

        if (shrinesVisited >= 3)
        {
            mapInfoLogHeader = "Call of the temple";
            mapInfoLogText = "Our holy place is calling us right now... ";
        }
        else
        {
            switch (Random.Range(0, 2))
            {
                case 0:
                    mapInfoLogHeader = "Energy resonates in the desert";
                    break;

                case 1:
                    mapInfoLogHeader = "Echo of our ancestors";
                    break;

                case 2:
                    mapInfoLogHeader = "Elder ruins call";
                    break;
            }

            switch (Random.Range(0, 2))
            {
                case 0:
                    mapInfoLogText = "I hear the voice calling us to pray...";
                    break;

                case 1:
                    mapInfoLogText = "We must find our bloody roots...";
                    break;

                case 2:
                    mapInfoLogText = "Let's open the gate to our destiny...";
                    break;
            }
        }

        InfoLog.Instance.ShowLogForSeconds(mapInfoLogText, mapInfoLogHeader, 10.0f);
    }

    public void UpdateInfoLog()
    {
        //Nothing to do here
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
