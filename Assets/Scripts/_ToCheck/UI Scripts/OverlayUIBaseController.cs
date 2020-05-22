using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayUIBaseController : MonoBehaviour
{
    private void OnEnable()
    {
        UIOverlayManager.Instance.ControlsSheet.AddSheetElement(ButtonActionType.UICancel, "Go back");

        ApplicationManager.Instance.Input.UI.Cancel.performed += ctx => Back();
    }

    private void OnDisable()
    {
        UIOverlayManager.Instance?.ControlsSheet.RemoveSheetElement(ButtonActionType.UICancel);

        ApplicationManager.Instance.Input.UI.Cancel.performed -= ctx => Back();
    }
    
    public void Back()
    {
        UIOverlayManager.Instance.PopFromCanvas();
    }
}
