using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public void Continue()
    {
        GameplayManager.Instance.ResumeGame();
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
