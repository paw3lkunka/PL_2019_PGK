using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class RebinderButton : MonoBehaviour
{
    public ButtonActionType actionType;
    public BindingPriority bindingPriority;

    public InputActionRebindingExtensions.RebindingOperation rebindOperation;

#region MonoBehaviour

    private void Start()
    {
        UpdateButtonIcon();
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

    public void Rebind()
    {
        int prior = 0;
        switch(bindingPriority)
        {
            case BindingPriority.Main:
                prior = 0;
                break;
            
            case BindingPriority.Alternative:
                prior = 1;
                break;
        }
        
        string scheme = "";
        switch(ApplicationManager.Instance.CurrentInputScheme)
        {
            case InputSchemeEnum.MouseKeyboard:
                scheme = "Mouse";
                break;

            case InputSchemeEnum.Gamepad:
                scheme = "Gamepad";
                break;
            
            case InputSchemeEnum.JoystickKeyboard:
                scheme = "Joystick";
                break;
        }

        InputAction action = null;
        switch(actionType)
        {
            case ButtonActionType.Walk:
                action = ApplicationManager.Instance.Input.Gameplay.SetWalkTarget;
                break;

            case ButtonActionType.Shoot:
                action = ApplicationManager.Instance.Input.CombatMode.SetShootTarget;
                break;

            case ButtonActionType.Interact:
                action = ApplicationManager.Instance.Input.Gameplay.ShowHideInfoLog;
                break;

            case ButtonActionType.Pause:
                action = ApplicationManager.Instance.Input.Gameplay.Pause;
                break;
        }

        var bindings = action.bindings.ToList();

        var bindingIndex = action.bindings
            .Where(b => b.groups.Contains(scheme))
            .Select(b => bindings.IndexOf(b))
            .ToArray()[prior];

        rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
            .OnCancel
            (
                operation =>
                {
                    //m_RebindStopEvent?.Invoke(this, operation);
                    //m_RebindOverlay?.SetActive(false);
                    UIOverlayManager.Instance.ControlsSheet.UpdateButton(actionType, bindingPriority);
                    UpdateButtonIcon();
                    rebindOperation.Dispose();
                    rebindOperation = null;
                }
            )
            .OnComplete
            (
                operation =>
                {
                    //m_RebindOverlay?.SetActive(false);
                    //m_RebindStopEvent?.Invoke(this, operation);
                    UIOverlayManager.Instance.ControlsSheet.UpdateButton(actionType, bindingPriority);
                    UpdateButtonIcon();
                    rebindOperation.Dispose();
                    rebindOperation = null;
                }
            );

        rebindOperation.Start();
    }

    private void UpdateButtonIcon()
    {
        UIOverlayManager.Instance.ControlsSheet
            .ShowButtonIcon
            (
                actionType,
                GetComponentInChildren<Image>(),
                GetComponentInChildren<TextMeshProUGUI>(),
                bindingPriority
            );
    }

#endregion

#region EventHandlers

    private void InputSchemeChangeHandler(PlayerInput obj)
    {
        UpdateButtonIcon();
    }

#endregion
}