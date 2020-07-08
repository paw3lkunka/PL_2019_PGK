using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatCursorManager : Singleton<CombatCursorManager, ForbidLazyInstancing>
{
    [field: SerializeField, GUIName("CursorRange")]
    public float CursorRange { get; private set; } = 5.0f;

    [HideInInspector]
    public GameObject MainCursor { get; private set; }
    private MeshRenderer cursorRenderer;
    
    public GameObject walkTargetIndicator;
    public GameObject shootTargetIndicator;

    private Camera mainCamera;

    #region MonoBehaviour

    protected void Start()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable() 
    {
        InitializeCursor();

        ApplicationManager.Instance.Input.Gameplay.SetWalkTarget.performed += SetWalkTargetIndicator;
        ApplicationManager.Instance.Input.Gameplay.SetWalkTarget.Enable();

        if (LocationManager.Instance.sceneMode == LocationMode.Hostile)
        {
            ApplicationManager.Instance.Input.CombatMode.SetShootTarget.performed += SetShootTargetIndicator;
            ApplicationManager.Instance.Input.CombatMode.SetShootTarget.Enable();
        }
    }

    private void OnDisable() 
    {
        if (ApplicationManager.Instance.Input != null)
        {
            ApplicationManager.Instance.Input.Gameplay.SetWalkTarget.performed -= SetWalkTargetIndicator;
            ApplicationManager.Instance.Input.Gameplay.SetWalkTarget.Disable();

            if (LocationManager.Instance.sceneMode == LocationMode.Hostile)
            {
                ApplicationManager.Instance.Input.CombatMode.SetShootTarget.performed -= SetShootTargetIndicator;
                ApplicationManager.Instance.Input.CombatMode.SetShootTarget.Disable();
            }
        }
    }

    private void Update() 
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

        // TODO: Cursor fade on distance to cult leader
    }

#endregion
    

#region PrivateManagerMethods
    private void InitializeCursor()
    {
        Cursor.visible = false;
        MainCursor = Instantiate(   ApplicationManager.Instance.PrefabDatabase.cursorPrefab, 
                                        new Vector3(0.0f, 0.0f, 0.0f), 
                                        Quaternion.identity);

        cursorRenderer = MainCursor.GetComponent<MeshRenderer>();
    }

    private void MoveCursorPointer()
    {
        Vector3 pos = default;
        if (SGUtils.CameraToGrounRaycast(Camera.main, 1000, ref pos))
        {
            MainCursor.transform.position = pos;
        }
    }

    private void MoveCursorGamepad()
    {
        var joystickAxis = Gamepad.current.leftStick.ReadValue();
        var nextCursorPosition = LocationManager.Instance.cultLeader.transform.position
                                    + new Vector3(joystickAxis.x, joystickAxis.y) * CursorRange;
        nextCursorPosition.z = 0;

        MainCursor.transform.position = nextCursorPosition;
    }

    private void MoveCursorJoystick()
    {
        var joystickAxis = Joystick.current.stick.ReadValue();
        var nextCursorPosition = LocationManager.Instance.cultLeader.transform.position
                                    + new Vector3(joystickAxis.x, joystickAxis.y) * CursorRange;
        nextCursorPosition.z = 0;
        MainCursor.transform.position = nextCursorPosition;
    }

    private void SetWalkTargetIndicator(InputAction.CallbackContext ctx)
    {
        // TODO: Check mouse over gui
        walkTargetIndicator.transform.position = MainCursor.transform.position;
    }

    private void SetShootTargetIndicator(InputAction.CallbackContext ctx)
    {
        shootTargetIndicator.transform.position = MainCursor.transform.position;
    }
    
#endregion
}
