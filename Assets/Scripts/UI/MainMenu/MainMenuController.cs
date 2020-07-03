using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void OnEnable()
    {
        UIOverlayManager.Instance.ControlsSheet?.Clear();
        UIOverlayManager.Instance.ControlsSheet?.AddSheetElement(ButtonActionType.UINavigate, "Navigate");
        UIOverlayManager.Instance.ControlsSheet?.AddSheetElement(ButtonActionType.UISubmit, "Submit");
    }

    public void Play()
    {
        UIOverlayManager.Instance.PushToCanvas(ApplicationManager.Instance.PrefabDatabase.difficultyMenuGUI, PushBehaviour.Hide);
    }

    public void Options()
    {
        UIOverlayManager.Instance.PushToCanvas(ApplicationManager.Instance.PrefabDatabase.optionsMenuGUI, PushBehaviour.Lock);
    }

    public void MainMenu()
    {
        ApplicationManager.Instance.MainMenu();
    }

    public void Exit()
    {
        ApplicationManager.Instance.ExitGame();
    }
}
