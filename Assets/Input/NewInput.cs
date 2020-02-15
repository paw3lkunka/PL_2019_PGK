// GENERATED AUTOMATICALLY FROM 'Assets/Input/NewInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @NewInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @NewInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""NewInput"",
    ""maps"": [
        {
            ""name"": ""CombatMode"",
            ""id"": ""3e4e1317-a05d-432b-ba93-9ba5a123c632"",
            ""actions"": [
                {
                    ""name"": ""SetShootTarget"",
                    ""type"": ""Button"",
                    ""id"": ""f852d4ee-ebdc-4263-b8eb-9423cf3bacdc"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c4e49b92-25e3-4839-b7e2-9c0555c9b658"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SetShootTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5d14c759-aaf3-4718-90e9-67958ef52f96"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SetShootTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b4e107d0-5298-44a8-b313-38f6c3208f76"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseKeyboard"",
                    ""action"": ""SetShootTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Gameplay"",
            ""id"": ""927fdd73-c6de-4111-8477-5fb2d3cd12f6"",
            ""actions"": [
                {
                    ""name"": ""MoveCursor"",
                    ""type"": ""Button"",
                    ""id"": ""ca9e273c-fe69-4b34-9937-3cfec5ef9452"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SetWalkTarget"",
                    ""type"": ""Button"",
                    ""id"": ""5099243b-0a5d-4979-830c-5104fc7c9a30"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b135ee91-571b-4fb8-bffa-75db907c22b6"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""Normalize(min=-1,max=1),AxisDeadzone"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MoveCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b87dc4d2-476f-44b4-abf8-b21dae40629f"",
                    ""path"": ""<Pointer>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseKeyboard"",
                    ""action"": ""MoveCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""671bf64e-70bf-4f6e-95c2-ea454a7e049f"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SetWalkTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e2e4e655-ae24-4f9d-a0bf-99f76d079ecb"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SetWalkTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""babcf9de-2fe9-4309-8c7b-b8f86a8fe898"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""MouseKeyboard"",
                    ""action"": ""SetWalkTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""MouseKeyboard"",
            ""bindingGroup"": ""MouseKeyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touchscreen"",
            ""bindingGroup"": ""Touchscreen"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // CombatMode
        m_CombatMode = asset.FindActionMap("CombatMode", throwIfNotFound: true);
        m_CombatMode_SetShootTarget = m_CombatMode.FindAction("SetShootTarget", throwIfNotFound: true);
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_MoveCursor = m_Gameplay.FindAction("MoveCursor", throwIfNotFound: true);
        m_Gameplay_SetWalkTarget = m_Gameplay.FindAction("SetWalkTarget", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // CombatMode
    private readonly InputActionMap m_CombatMode;
    private ICombatModeActions m_CombatModeActionsCallbackInterface;
    private readonly InputAction m_CombatMode_SetShootTarget;
    public struct CombatModeActions
    {
        private @NewInput m_Wrapper;
        public CombatModeActions(@NewInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @SetShootTarget => m_Wrapper.m_CombatMode_SetShootTarget;
        public InputActionMap Get() { return m_Wrapper.m_CombatMode; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CombatModeActions set) { return set.Get(); }
        public void SetCallbacks(ICombatModeActions instance)
        {
            if (m_Wrapper.m_CombatModeActionsCallbackInterface != null)
            {
                @SetShootTarget.started -= m_Wrapper.m_CombatModeActionsCallbackInterface.OnSetShootTarget;
                @SetShootTarget.performed -= m_Wrapper.m_CombatModeActionsCallbackInterface.OnSetShootTarget;
                @SetShootTarget.canceled -= m_Wrapper.m_CombatModeActionsCallbackInterface.OnSetShootTarget;
            }
            m_Wrapper.m_CombatModeActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SetShootTarget.started += instance.OnSetShootTarget;
                @SetShootTarget.performed += instance.OnSetShootTarget;
                @SetShootTarget.canceled += instance.OnSetShootTarget;
            }
        }
    }
    public CombatModeActions @CombatMode => new CombatModeActions(this);

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_MoveCursor;
    private readonly InputAction m_Gameplay_SetWalkTarget;
    public struct GameplayActions
    {
        private @NewInput m_Wrapper;
        public GameplayActions(@NewInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveCursor => m_Wrapper.m_Gameplay_MoveCursor;
        public InputAction @SetWalkTarget => m_Wrapper.m_Gameplay_SetWalkTarget;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @MoveCursor.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMoveCursor;
                @MoveCursor.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMoveCursor;
                @MoveCursor.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMoveCursor;
                @SetWalkTarget.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSetWalkTarget;
                @SetWalkTarget.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSetWalkTarget;
                @SetWalkTarget.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSetWalkTarget;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveCursor.started += instance.OnMoveCursor;
                @MoveCursor.performed += instance.OnMoveCursor;
                @MoveCursor.canceled += instance.OnMoveCursor;
                @SetWalkTarget.started += instance.OnSetWalkTarget;
                @SetWalkTarget.performed += instance.OnSetWalkTarget;
                @SetWalkTarget.canceled += instance.OnSetWalkTarget;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    private int m_MouseKeyboardSchemeIndex = -1;
    public InputControlScheme MouseKeyboardScheme
    {
        get
        {
            if (m_MouseKeyboardSchemeIndex == -1) m_MouseKeyboardSchemeIndex = asset.FindControlSchemeIndex("MouseKeyboard");
            return asset.controlSchemes[m_MouseKeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_TouchscreenSchemeIndex = -1;
    public InputControlScheme TouchscreenScheme
    {
        get
        {
            if (m_TouchscreenSchemeIndex == -1) m_TouchscreenSchemeIndex = asset.FindControlSchemeIndex("Touchscreen");
            return asset.controlSchemes[m_TouchscreenSchemeIndex];
        }
    }
    public interface ICombatModeActions
    {
        void OnSetShootTarget(InputAction.CallbackContext context);
    }
    public interface IGameplayActions
    {
        void OnMoveCursor(InputAction.CallbackContext context);
        void OnSetWalkTarget(InputAction.CallbackContext context);
    }
}
