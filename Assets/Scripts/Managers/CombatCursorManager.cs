using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatCursorManager : Singleton<CombatCursorManager, ForbidLazyInstancing>
{
    [field: SerializeField, GUIName("CursorRange")]
    public float cursorDeadzone { get; private set; } = 0.7f;

    [field: SerializeField, GUIName("CursorRange")]
    public float CursorRange { get; private set; } = 12.0f;

    [field: SerializeField, GUIName("ShootingCancelRange")]
    public float ShootingCancelRange { get; private set; } = 5.0f;

    [HideInInspector]
    public GameObject MainCursor { get; private set; }
    private MeshRenderer cursorRenderer;
    
    public GameObject walkTargetIndicator;
    public GameObject shootTargetIndicator;
    public GameObject shootTargetPositionIndicator;
    public GameObject shootCancelRange;

    public Vector3 shootDirection;
    public float shootHalfAngle;

    /// <summary>
    /// Is shootTargetIndicator placed
    /// </summary>
    [SerializeField]
    private bool canShoot = false;
    public bool CanShoot => canShoot;

    private Camera mainCamera;

    private bool isJoystickLeanOut = false;

    #region MonoBehaviour

    protected void Start()
    {
        InitShootTargetPositionIndicator();
        mainCamera = Camera.main;
        shootCancelRange.transform.localScale = new Vector3(ShootingCancelRange * 2, 0.3f, ShootingCancelRange * 2);
    }

    private void OnEnable() 
    {
        InitializeCursor();

        ApplicationManager.Instance.Input.Gameplay.SetWalkTarget.performed += SetWalkTargetIndicator;
        ApplicationManager.Instance.Input.Gameplay.SetWalkTarget.Enable();

        ApplicationManager.Instance.Input.CombatMode.SetShootTarget.performed += SetShootTargetIndicator;
        
        if (LocationManager.Instance.sceneMode == LocationMode.Hostile)
        {
            ApplicationManager.Instance.Input.CombatMode.SetShootTarget.Enable();
        }
    }

    private void OnDisable() 
    {
        if (ApplicationManager.Instance.Input != null)
        {
            ApplicationManager.Instance.Input.Gameplay.SetWalkTarget.performed -= SetWalkTargetIndicator;
            ApplicationManager.Instance.Input.Gameplay.SetWalkTarget.Disable();

            ApplicationManager.Instance.Input.CombatMode.SetShootTarget.performed -= SetShootTargetIndicator;
            ApplicationManager.Instance.Input.CombatMode.SetShootTarget.Disable();
        }
    }

    private void Update() 
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

            MoveShootTargetPositionIndicator();
            // TODO: Cursor fade on distance to cult leader
        }
    }

#endregion
    

#region PrivateManagerMethods
    private void InitializeCursor()
    {
        //Cursor.visible = false;
        MainCursor = Instantiate(   ApplicationManager.Instance.PrefabDatabase.cursorPrefab, 
                                        new Vector3(0.0f, 0.0f, 0.0f), 
                                        Quaternion.identity);

        cursorRenderer = MainCursor.GetComponent<MeshRenderer>();
        canShoot = false;
    }

    private void MoveCursorPointer()
    {
        Vector3 pos = default;
        if (SGUtils.CameraToGroundNearestRaycast(Camera.main, 1000, ref pos))
        {
            MainCursor.transform.position = pos;
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
        var nextCursorPosition = LocationManager.Instance.cultLeader.transform.position;

        if(joystickAxis.magnitude > cursorDeadzone)
        {
            isJoystickLeanOut = true;
            var cursorOffset = new Vector3(-joystickAxis.y, 0.0f, joystickAxis.x) / joystickAxis.magnitude * CursorRange;
            nextCursorPosition += cursorOffset;
            
            RaycastHit rayHit;
            Physics.Raycast(new Vector3(nextCursorPosition.x, 1000.0f, nextCursorPosition.z), Vector3.down, out rayHit);
            nextCursorPosition = rayHit.point;
        }
        else
        {
            isJoystickLeanOut = false;
        }

        MainCursor.transform.position = nextCursorPosition;
    }

    private void InitShootTargetPositionIndicator()
    {
        shootTargetPositionIndicator.transform.SetParent(LocationManager.Instance.cultLeader.transform, false);
        shootTargetIndicator.transform.SetParent(LocationManager.Instance.cultLeader.transform, false);
        shootCancelRange.transform.SetParent(LocationManager.Instance.cultLeader.transform, false);
        shootTargetIndicator.SetActive(false);
    }

    private void MoveShootTargetPositionIndicator()
    {
        var direction = (MainCursor.transform.position - LocationManager.Instance.cultLeader.transform.position).normalized;
        // This is basically a hack, but for some reason Quaternion.LookRotation didn't give a shit that we always want to rotate according to Vector3.up
        shootTargetPositionIndicator.transform.rotation = Quaternion.Euler(0.0f, Quaternion.LookRotation(direction, Vector3.up).eulerAngles.y, 0.0f);
    }

    private void SetWalkTargetIndicator(InputAction.CallbackContext ctx)
    {
        // TODO: Check mouse over gui
        switch (ApplicationManager.Instance.CurrentInputScheme)
            {
                case InputSchemeEnum.MouseKeyboard:
                    walkTargetIndicator.transform.position = MainCursor.transform.position;
                    break;

                case InputSchemeEnum.Gamepad:
                case InputSchemeEnum.JoystickKeyboard:
                    var cultLeaderPosition = LocationManager.Instance.cultLeader.transform.position;
                    if(isJoystickLeanOut)
                    {
                        var cursorPos = MainCursor.transform.position;
                        cultLeaderPosition.y += 2.0f;
                        cursorPos.y += 2.0f;

                        RaycastHit rayHit;
                        int layerMask = LayerMask.GetMask("Default");
                        if(Physics.Raycast(cultLeaderPosition, cursorPos - cultLeaderPosition , out rayHit, Mathf.Infinity, layerMask))
                        {
                            cursorPos = Vector3.MoveTowards(rayHit.point, cultLeaderPosition, 0.5f);
                            cursorPos.y = 1000.0f;
                            if(Physics.Raycast(cursorPos, Vector3.down, out rayHit, Mathf.Infinity, layerMask))
                            {
                                cursorPos = rayHit.point;
                            }
                        }
                        else
                        {
                            cursorPos = MainCursor.transform.position;
                            if(Physics.Raycast(cursorPos, Vector3.down, out rayHit, Mathf.Infinity, layerMask))
                            {
                                cursorPos = rayHit.point;
                            }
                        }

                        walkTargetIndicator.transform.position = cursorPos;
                    }
                    else
                    {
                        walkTargetIndicator.transform.position = cultLeaderPosition;
                    }
                    break;

                case InputSchemeEnum.Touchscreen:
                    break;
            }
    }

    private void SetShootTargetIndicator(InputAction.CallbackContext ctx)
    {
        float cursorDistance = Vector3.Distance(MainCursor.transform.position, LocationManager.Instance.cultLeader.transform.position);

        if(!canShoot && cursorDistance > ShootingCancelRange)
        {
            canShoot = true;
        }
        else if(cursorDistance < ShootingCancelRange)
        {
            canShoot = false;
        }

        shootTargetIndicator.SetActive(canShoot);
        shootDirection = (MainCursor.transform.position - LocationManager.Instance.cultLeader.transform.position).normalized;
        shootTargetIndicator.transform.rotation = Quaternion.LookRotation(shootDirection, Vector3.up);
    }
    
#endregion
}
