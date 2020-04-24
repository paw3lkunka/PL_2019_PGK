using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void Play()
    {
        UIOverlayManager.Instance.PushToCanvas(ApplicationManager.Instance.prefabDatabase.difficultyMenuGUI, true);
    }

    public void Options()
    {
        UIOverlayManager.Instance.PushToCanvas(ApplicationManager.Instance.prefabDatabase.optionsMenuGUI, false);
    }
}
