using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public void Continue()
    {
        Time.timeScale = 1.0f;
        AudioTimeline.Instance.Resume();
        UIOverlayManager.Instance.PopFromCanvas();
    }

    public void Options()
    {
        UIOverlayManager.Instance.PushToCanvas(ApplicationManager.Instance.PrefabDatabase.optionsMenuGUI, PushBehaviour.Lock);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1.0f;
        ApplicationManager.Instance.MainMenu();
    }
}
