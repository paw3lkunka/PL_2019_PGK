using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class ControlsSheet : MonoBehaviour
{
    [SerializeField]
    public GameObject sheetElement;
    private List<ActionDescription> currentSheetElements = new List<ActionDescription>();

    [Header("Keyboard bindings")]
    public KeyboardButtons[] walkMouseKeyboardBinding = { KeyboardButtons.unknown, KeyboardButtons.unknown };
    public KeyboardButtons[] shootMouseKeyboardBinding = { KeyboardButtons.unknown, KeyboardButtons.unknown };
    public KeyboardButtons[] interactMouseKeyboardBinding = { KeyboardButtons.unknown, KeyboardButtons.unknown };
    public KeyboardButtons[] pauseMouseKeyboardBinding = { KeyboardButtons.unknown, KeyboardButtons.unknown };
    public Sprite cameraMoveMouseKeyboardBinding;
    public Sprite cameraZoomMouseKeyboardBinding;
    public Sprite uiNavigateMouseKeyboardBinding;
    public Sprite uiSubmitMouseKeyboardBinding;
    public Sprite uiCancelMouseKeyboardBinding;

    [Header("Keyboard bindings")]
    public KeyboardButtons[] walkJoystickKeyboardBinding = { KeyboardButtons.unknown, KeyboardButtons.unknown };
    public KeyboardButtons[] shootJoystickKeyboardBinding = { KeyboardButtons.unknown, KeyboardButtons.unknown };
    public KeyboardButtons[] interactJoystickKeyboardBinding = { KeyboardButtons.unknown, KeyboardButtons.unknown };
    public KeyboardButtons[] pauseJoystickKeyboardBinding = { KeyboardButtons.unknown, KeyboardButtons.unknown };
    public Sprite cameraMoveJoystickKeyboardBinding;
    public Sprite cameraZoomJoystickKeyboardBinding;
    public Sprite uiNavigateJoystickKeyboardBinding;
    public Sprite uiSubmitJoystickKeyboardBinding;
    public Sprite uiCancelJoystickKeyboardBinding;

    [Header("Gamepad bindings")]
    public GamepadButtons[] walkGamepadBinding = { GamepadButtons.unknown, GamepadButtons.unknown };
    public GamepadButtons[] shootGamepadBinding = { GamepadButtons.unknown, GamepadButtons.unknown };
    public GamepadButtons[] interactGamepadBinding = { GamepadButtons.unknown, GamepadButtons.unknown };
    public GamepadButtons[] pauseGamepadBinding = { GamepadButtons.unknown, GamepadButtons.unknown };
    public Sprite cameraMoveGamepadBinding;
    public Sprite cameraZoomGamepadBinding;
    public Sprite uiNavigateGamepadBinding;
    public Sprite uiSubmitGamepadBinding;
    public Sprite uiCancelGamepadBinding;

    [Header("Sprites & Captions")]
    public Sprite standardKeyboardSprite;
    [SerializeField]
    public List<NormalKeyboardChanged> normalKeyboardChanges;
    [SerializeField]
    public List<SpecialKeyboardSprite> specialKeyboardSprites;
    [SerializeField]
    public List<GamepadButtonSprite> gamepadSprites;

    private NewInput input;

#region MonoBehaviour

    private void Awake() 
    {
        input = ApplicationManager.Instance.Input;

        shootMouseKeyboardBinding = GetBindingsEnumsForAction<KeyboardButtons>(input.Gameplay.SecondaryAction, "Mouse");
        shootJoystickKeyboardBinding = GetBindingsEnumsForAction<KeyboardButtons>(input.Gameplay.SecondaryAction, "Joystick");
        shootGamepadBinding = GetBindingsEnumsForAction<GamepadButtons>(input.Gameplay.SecondaryAction, "Gamepad");

        walkMouseKeyboardBinding = GetBindingsEnumsForAction<KeyboardButtons>(input.Gameplay.PrimaryAction, "Mouse");
        walkJoystickKeyboardBinding = GetBindingsEnumsForAction<KeyboardButtons>(input.Gameplay.PrimaryAction, "Joystick");
        walkGamepadBinding = GetBindingsEnumsForAction<GamepadButtons>(input.Gameplay.PrimaryAction, "Gamepad");

        interactMouseKeyboardBinding = GetBindingsEnumsForAction<KeyboardButtons>(input.Gameplay.Interact, "Mouse");
        interactJoystickKeyboardBinding = GetBindingsEnumsForAction<KeyboardButtons>(input.Gameplay.Interact, "Joystick");
        interactGamepadBinding = GetBindingsEnumsForAction<GamepadButtons>(input.Gameplay.Interact, "Gamepad");

        pauseMouseKeyboardBinding = GetBindingsEnumsForAction<KeyboardButtons>(input.Gameplay.Pause, "Mouse");
        pauseJoystickKeyboardBinding = GetBindingsEnumsForAction<KeyboardButtons>(input.Gameplay.Pause, "Joystick");
        pauseGamepadBinding = GetBindingsEnumsForAction<GamepadButtons>(input.Gameplay.Pause, "Gamepad");
    }

    private void OnEnable()
    {
        ApplicationManager.Instance.InputSchemeChange += InputSchemeChangeHandler;
    }

    private void OnDisable()
    {
        ApplicationManager.Instance.InputSchemeChange -= InputSchemeChangeHandler;
    }

#endregion

#region Component

    public void AddSheetElement(ButtonActionType type, string name)
    {
        RemoveSheetElement(type);

        var newSheetDescription = new ActionDescription()
        {
            type = type,
            name = name
        };

        currentSheetElements.Add(newSheetDescription);
        ShowSheetElement(type, name);
    }

    public void ShowSheetElement(ButtonActionType type, string name)
    {
        var newElement = (GameObject) Instantiate(sheetElement, transform);

        newElement.transform.GetComponentInChildren<TextMeshProUGUI>().text = name;

        ShowButtonIcon
        (
            type,
            newElement.transform.GetChild(1).GetComponentInChildren<Image>(),
            newElement.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>()
        );
    }

    public void ShowButtonIcon(ButtonActionType type, Image image, TextMeshProUGUI textMesh, BindingPriority priority = BindingPriority.Main)
    {
        int prior = 0;
        switch(priority)
        {
            case BindingPriority.Main:
                prior = 0;
                break;

            case BindingPriority.Alternative:
                prior = 1;
                break;
        }

        switch(ApplicationManager.Instance.CurrentInputScheme)
        {
            case InputSchemeEnum.MouseKeyboard:

                KeyboardButtons keyboardButton = KeyboardButtons.unknown;

                switch(type)
                {
                    case ButtonActionType.Walk:
                        keyboardButton = walkMouseKeyboardBinding[prior];
                        break;
                    
                    case ButtonActionType.Shoot:
                        keyboardButton = shootMouseKeyboardBinding[prior];
                        break;
                    
                    case ButtonActionType.Interact:
                        keyboardButton = interactMouseKeyboardBinding[prior];
                        break;
                    
                    case ButtonActionType.Pause:
                        keyboardButton = pauseMouseKeyboardBinding[prior];
                        break;

                    case ButtonActionType.CameraMove:
                        image.sprite = cameraMoveMouseKeyboardBinding;
                        textMesh.text = "";
                        return;

                    case ButtonActionType.CameraZoom:
                        image.sprite = cameraZoomMouseKeyboardBinding;
                        textMesh.text = "";
                        return;

                    case ButtonActionType.UINavigate:
                        image.sprite = uiNavigateMouseKeyboardBinding;
                        textMesh.text = "";
                        return;

                    case ButtonActionType.UISubmit:
                        image.sprite = uiSubmitMouseKeyboardBinding;
                        textMesh.text = "";
                        return;

                    case ButtonActionType.UICancel:
                        image.sprite = uiCancelMouseKeyboardBinding;
                        textMesh.text = "";
                        return;
                }

                if(keyboardButton >= KeyboardButtons.first_standard_noChange && keyboardButton <= KeyboardButtons.last_standard_noChange)
                {
                    image.sprite = standardKeyboardSprite;
                    textMesh.text = keyboardButton.ToString().ToUpper();
                }
                else if(keyboardButton >= KeyboardButtons.first_standard_change && keyboardButton <= KeyboardButtons.last_standard_change)
                {
                    image.sprite = standardKeyboardSprite;
                    textMesh.text = normalKeyboardChanges.FirstOrDefault(c => c.button.Equals(keyboardButton)).text;
                }
                else if(keyboardButton >= KeyboardButtons.first_special && keyboardButton <= KeyboardButtons.last_special)
                {
                    image.sprite = specialKeyboardSprites.FirstOrDefault(c => c.button.Equals(keyboardButton)).image;
                    textMesh.text = "";
                }

                break;

            case InputSchemeEnum.JoystickKeyboard:

                KeyboardButtons joystickButton = KeyboardButtons.unknown;

                switch(type)
                {
                    case ButtonActionType.Walk:
                        joystickButton = walkJoystickKeyboardBinding[prior];
                        break;
                    
                    case ButtonActionType.Shoot:
                        joystickButton = shootJoystickKeyboardBinding[prior];
                        break;
                    
                    case ButtonActionType.Interact:
                        joystickButton = interactJoystickKeyboardBinding[prior];
                        break;
                    
                    case ButtonActionType.Pause:
                        joystickButton = pauseJoystickKeyboardBinding[prior];
                        break;
                        
                     case ButtonActionType.CameraMove:
                        image.sprite = cameraMoveMouseKeyboardBinding;
                        textMesh.text = "";
                        return;

                    case ButtonActionType.CameraZoom:
                        image.sprite = cameraZoomMouseKeyboardBinding;
                        textMesh.text = "";
                        return;

                    case ButtonActionType.UINavigate:
                        image.sprite = uiNavigateJoystickKeyboardBinding;
                        textMesh.text = "";
                        return;

                    case ButtonActionType.UISubmit:
                        image.sprite = uiSubmitJoystickKeyboardBinding;
                        textMesh.text = "";
                        return;

                    case ButtonActionType.UICancel:
                        image.sprite = uiCancelJoystickKeyboardBinding;
                        textMesh.text = "";
                        break;
                }

                if(joystickButton >= KeyboardButtons.first_standard_noChange && joystickButton <= KeyboardButtons.last_standard_noChange)
                {
                    image.sprite = standardKeyboardSprite;
                    textMesh.text = joystickButton.ToString();
                }
                else if(joystickButton >= KeyboardButtons.first_standard_noChange && joystickButton <= KeyboardButtons.last_standard_noChange)
                {
                    image.sprite = standardKeyboardSprite;
                    textMesh.text = normalKeyboardChanges.FirstOrDefault(c => c.button.Equals(joystickButton)).text;
                }
                else if(joystickButton >= KeyboardButtons.first_special && joystickButton <= KeyboardButtons.last_special)
                {
                    image.sprite = specialKeyboardSprites.FirstOrDefault(c => c.button.Equals(joystickButton)).image;
                    textMesh.text = "";
                }
            
                break;

            case InputSchemeEnum.Gamepad:

                GamepadButtons gamepadButton = GamepadButtons.unknown;

                switch(type)
                {
                    case ButtonActionType.Walk:
                        gamepadButton = walkGamepadBinding[prior];
                        break;
                    
                    case ButtonActionType.Shoot:
                        gamepadButton = shootGamepadBinding[prior];
                        break;
                    
                    case ButtonActionType.Interact:
                        gamepadButton = interactGamepadBinding[prior];
                        break;
                    
                    case ButtonActionType.Pause:
                        gamepadButton = pauseGamepadBinding[prior];
                        break;

                    case ButtonActionType.CameraMove:
                        image.sprite = cameraMoveGamepadBinding;
                        textMesh.text = "";
                        return;

                    case ButtonActionType.CameraZoom:
                        image.sprite = cameraZoomGamepadBinding;
                        textMesh.text = "";
                        return;

                    case ButtonActionType.UINavigate:
                        image.sprite = uiNavigateGamepadBinding;
                        textMesh.text = "";
                        return;

                    case ButtonActionType.UISubmit:
                        image.sprite = uiSubmitGamepadBinding;
                        textMesh.text = "";
                        return;

                    case ButtonActionType.UICancel:
                        image.sprite = uiCancelGamepadBinding;
                        textMesh.text = "";
                        return;
                }

                image.sprite = gamepadSprites.FirstOrDefault(c => c.button.Equals(gamepadButton)).image;
                textMesh.text = "";

                break;
        }
    }

    public void RemoveSheetElement(ButtonActionType type)
    {
        var index = currentSheetElements.FindIndex(e => e.type.Equals(type));
        if(index == -1)
        {
            return;
        }

        var element = transform.GetChild(index).gameObject;
        Destroy(element);
        currentSheetElements.RemoveAt(index);
    }

    public void Clear()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        
        currentSheetElements.Clear();
    }

    public void UpdateButton(ButtonActionType action, BindingPriority priority)
    {
        int prior = 0;
        switch(priority)
        {
            case BindingPriority.Main:
                prior = 0;
                break;
            
            case BindingPriority.Alternative:
                prior = 1;
                break;
        }

        switch(ApplicationManager.Instance.CurrentInputScheme)
        {
            case InputSchemeEnum.MouseKeyboard:
                switch(action)
                {
                    case ButtonActionType.Walk:
                        walkMouseKeyboardBinding[prior] = GetBindingsEnumsForAction<KeyboardButtons>(input.Gameplay.PrimaryAction, "Mouse")[prior];
                        break;
                    
                    case ButtonActionType.Shoot:
                        shootMouseKeyboardBinding[prior] = GetBindingsEnumsForAction<KeyboardButtons>(input.Gameplay.SecondaryAction, "Mouse")[prior];
                        break;
                    
                    case ButtonActionType.Interact:
                        interactMouseKeyboardBinding[prior] = GetBindingsEnumsForAction<KeyboardButtons>(input.Gameplay.Interact, "Mouse")[prior];
                        break;
                    
                    case ButtonActionType.Pause:
                        pauseMouseKeyboardBinding[prior] = GetBindingsEnumsForAction<KeyboardButtons>(input.Gameplay.Pause, "Mouse")[prior];
                        break;
                }
                break;

            case InputSchemeEnum.Gamepad:
                switch(action)
                {
                    case ButtonActionType.Walk:
                        walkGamepadBinding[prior] = GetBindingsEnumsForAction<GamepadButtons>(input.Gameplay.PrimaryAction, "Gamepad")[prior];
                        break;
                    
                    case ButtonActionType.Shoot:
                        shootGamepadBinding[prior] = GetBindingsEnumsForAction<GamepadButtons>(input.Gameplay.SecondaryAction, "Gamepad")[prior];
                        break;
                    
                    case ButtonActionType.Interact:
                        interactGamepadBinding[prior] = GetBindingsEnumsForAction<GamepadButtons>(input.Gameplay.Interact, "Gamepad")[prior];
                        break;
                    
                    case ButtonActionType.Pause:
                        pauseGamepadBinding[prior] = GetBindingsEnumsForAction<GamepadButtons>(input.Gameplay.Pause, "Gamepad")[prior];
                        break;
                }
                break;

            case InputSchemeEnum.JoystickKeyboard:
                switch(action)
                {
                    case ButtonActionType.Walk:
                        walkJoystickKeyboardBinding[prior] = GetBindingsEnumsForAction<KeyboardButtons>(input.Gameplay.PrimaryAction, "Joystick")[prior];
                        break;
                    
                    case ButtonActionType.Shoot:
                        shootJoystickKeyboardBinding[prior] = GetBindingsEnumsForAction<KeyboardButtons>(input.Gameplay.SecondaryAction, "Joystick")[prior];
                        break;
                    
                    case ButtonActionType.Interact:
                        interactJoystickKeyboardBinding[prior] = GetBindingsEnumsForAction<KeyboardButtons>(input.Gameplay.Interact, "Joystick")[prior];
                        break;
                    
                    case ButtonActionType.Pause:
                        pauseJoystickKeyboardBinding[prior] = GetBindingsEnumsForAction<KeyboardButtons>(input.Gameplay.Pause, "Joystick")[prior];
                        break;
                }
                break;
        }
    }

    private T[] GetBindingsEnumsForAction<T>(InputAction action, string scheme) where T : struct, IConvertible
    {
        return action.bindings
            .Where(b => b.groups.Contains(scheme))
            .Select
            (
                b => 
                {
                    T buttonEnum;
                    Enum.TryParse<T>(b.path.Substring(b.path.LastIndexOf('/') + 1), false, out buttonEnum);
                    return buttonEnum;
                }
            )
            .ToArray();
    }

#endregion

#region EventHandlers

    private void InputSchemeChangeHandler(PlayerInput obj)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        
        foreach(var desc in currentSheetElements)
        {
            ShowSheetElement(desc.type, desc.name);
        }
    }

#endregion
}


[Serializable]
public struct SpecialKeyboardSprite 
{
    public KeyboardButtons button;
    public Sprite image;
}

[Serializable]
public struct NormalKeyboardChanged
{
    public KeyboardButtons button;
    public string text;
}

[Serializable]
public struct GamepadButtonSprite 
{
    public GamepadButtons button;
    public Sprite image;
}

public struct ActionDescription
{
    public ButtonActionType type;
    public string name;
}