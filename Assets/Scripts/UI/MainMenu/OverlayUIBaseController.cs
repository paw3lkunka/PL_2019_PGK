using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OverlayUIBaseController : MonoBehaviour
{
    [SerializeField] private bool closeable = false;
    private void Start()
    {
        if(closeable)
        {
            UIOverlayManager.Instance.ControlsSheet.AddSheetElement(ButtonActionType.UICancel, "Go back");
        }
    }

    private void OnEnable()
    {
        if(closeable)
        {
            ApplicationManager.Instance.Input.UI.Cancel.performed += UICancelHandler;
            ApplicationManager.Instance.Input.UI.Cancel.Enable();
        }
    }

    private void OnDisable()
    {
        if(closeable)
        {
            ApplicationManager.Instance.Input.UI.Cancel.performed -= UICancelHandler;
            ApplicationManager.Instance.Input.UI.Cancel.Disable();
        }
    }
    
    public void Back()
    {
        UIOverlayManager.Instance.PopFromCanvas();
    }

    private void UICancelHandler(InputAction.CallbackContext ctx)
    {
        Back();
    }
}
