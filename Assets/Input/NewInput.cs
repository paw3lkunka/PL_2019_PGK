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
                    ""groups"": """",
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
                    ""groups"": """",
                    ""action"": ""SetShootTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""MapMode"",
            ""id"": ""83a9ad21-0a7c-4505-8641-20b079eaed81"",
            ""actions"": [],
            ""bindings"": []
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
                    ""processors"": """",
                    ""groups"": """",
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
                    ""groups"": """",
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
                    ""groups"": """",
                    ""action"": ""SetWalkTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""GUI"",
            ""id"": ""d7a5d1d2-00e3-4295-8a0f-6ed4d282e171"",
            ""actions"": [],
            ""bindings"": []
        }
    ],
    ""controlSchemes"": []
}");
        // CombatMode
        m_CombatMode = asset.FindActionMap("CombatMode", throwIfNotFound: true);
        m_CombatMode_SetShootTarget = m_CombatMode.FindAction("SetShootTarget", throwIfNotFound: true);
        // MapMode
        m_MapMode = asset.FindActionMap("MapMode", throwIfNotFound: true);
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_MoveCursor = m_Gameplay.FindAction("MoveCursor", throwIfNotFound: true);
        m_Gameplay_SetWalkTarget = m_Gameplay.FindAction("SetWalkTarget", throwIfNotFound: true);
        // GUI
        m_GUI = asset.FindActionMap("GUI", throwIfNotFound: true);
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

    // MapMode
    private readonly InputActionMap m_MapMode;
    private IMapModeActions m_MapModeActionsCallbackInterface;
    public struct MapModeActions
    {
        private @NewInput m_Wrapper;
        public MapModeActions(@NewInput wrapper) { m_Wrapper = wrapper; }
        public InputActionMap Get() { return m_Wrapper.m_MapMode; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MapModeActions set) { return set.Get(); }
        public void SetCallbacks(IMapModeActions instance)
        {
            if (m_Wrapper.m_MapModeActionsCallbackInterface != null)
            {
            }
            m_Wrapper.m_MapModeActionsCallbackInterface = instance;
            if (instance != null)
            {
            }
        }
    }
    public MapModeActions @MapMode => new MapModeActions(this);

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

    // GUI
    private readonly InputActionMap m_GUI;
    private IGUIActions m_GUIActionsCallbackInterface;
    public struct GUIActions
    {
        private @NewInput m_Wrapper;
        public GUIActions(@NewInput wrapper) { m_Wrapper = wrapper; }
        public InputActionMap Get() { return m_Wrapper.m_GUI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GUIActions set) { return set.Get(); }
        public void SetCallbacks(IGUIActions instance)
        {
            if (m_Wrapper.m_GUIActionsCallbackInterface != null)
            {
            }
            m_Wrapper.m_GUIActionsCallbackInterface = instance;
            if (instance != null)
            {
            }
        }
    }
    public GUIActions @GUI => new GUIActions(this);
    public interface ICombatModeActions
    {
        void OnSetShootTarget(InputAction.CallbackContext context);
    }
    public interface IMapModeActions
    {
    }
    public interface IGameplayActions
    {
        void OnMoveCursor(InputAction.CallbackContext context);
        void OnSetWalkTarget(InputAction.CallbackContext context);
    }
    public interface IGUIActions
    {
    }
}
