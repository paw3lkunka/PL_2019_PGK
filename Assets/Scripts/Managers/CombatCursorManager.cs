using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatCursorManager : Singleton<CombatCursorManager>
{
#pragma warning disable
    [SerializeField] private float cursorRange = 5.0f;
#pragma warning restore

    private GameObject mainCursor;
    private MeshRenderer cursorRenderer;
    
    private GameObject walkTargetIndicator;
    private GameObject shootTargetIndicator;
    
    private NewInput input;
    
#region MonoBehaviour
    private void Start()
    {
        input = ApplicationManager.Instance.Input;
    }

    private void OnEnable() 
    {
        InitializeCursor();

        if (input != null)
        {
            input.Gameplay.SetWalkTarget.performed += SetWalkTargetIndicator;
            input.Gameplay.SetWalkTarget.Enable();

            if (CombatSceneManager.Instance.sceneMode == CombatSceneMode.Hostile)
            {
                input.CombatMode.SetShootTarget.performed += SetShootTargetIndicator;
                input.CombatMode.SetShootTarget.Enable();
            }
        }
    }

    private void OnDisable() 
    {
        if (input != null)
        {
            input.Gameplay.SetWalkTarget.performed -= SetWalkTargetIndicator;
            input.Gameplay.SetWalkTarget.Disable();

            if (CombatSceneManager.Instance.sceneMode == CombatSceneMode.Hostile)
            {
                input.CombatMode.SetShootTarget.performed -= SetShootTargetIndicator;
                input.CombatMode.SetShootTarget.Disable();
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
        mainCursor = Instantiate(   ApplicationManager.Instance.prefabDatabase.cursorPrefab, 
                                        CombatSceneManager.Instance.startPoint.position, 
                                        Quaternion.identity);

        cursorRenderer = mainCursor.GetComponent<MeshRenderer>();
    }

    private void MoveCursorPointer()
    {
        var inputValue = Mouse.current.position.ReadValue();
        var ray = Camera.main.ScreenPointToRay(inputValue);

        foreach( var hit in Physics.RaycastAll(ray, 100) )
        {
            // TODO: replace tag with layer mask
            if (hit.collider.CompareTag("Ground"))
            {
                mainCursor.transform.position = hit.point;
                continue;
            }
        }        
    }

    private void MoveCursorGamepad()
    {
        var joystickAxis = Gamepad.current.leftStick.ReadValue();
        var nextCursorPosition = CombatSceneManager.Instance.CultLeaderTransform.position
                                    + new Vector3(joystickAxis.x, joystickAxis.y) * cursorRange;
        nextCursorPosition.z = 0;

        mainCursor.transform.position = nextCursorPosition;
    }

    private void MoveCursorJoystick()
    {
        var joystickAxis = Joystick.current.stick.ReadValue();
        var nextCursorPosition = CombatSceneManager.Instance.CultLeaderTransform.position
                                    + new Vector3(joystickAxis.x, joystickAxis.y) * cursorRange;
        nextCursorPosition.z = 0;
        mainCursor.transform.position = nextCursorPosition;
    }

    private void SetWalkTargetIndicator(InputAction.CallbackContext ctx)
    {
        // TODO: Check mouse over gui
        walkTargetIndicator.transform.position = mainCursor.transform.position;
    }

    private void SetShootTargetIndicator(InputAction.CallbackContext ctx)
    {
        shootTargetIndicator.transform.position = mainCursor.transform.position;
    }
    
#endregion
}
