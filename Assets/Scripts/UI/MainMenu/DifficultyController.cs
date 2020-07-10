using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    public bool SkipTutorial { get; set; } = true;
    public bool EnableCheats { get; set; } = false;
    public bool DebugScene { get; set; } = false;

    public void Easy()
    {
        ApplicationManager.Instance.enableCheats = EnableCheats;
        ApplicationManager.Instance.skipTutorial = SkipTutorial;
        ApplicationManager.Instance.StartGame(DebugScene ? GameMode.Debug : GameMode.Normal, Difficulty.Easy);
    }

    public void Hard()
    {
        ApplicationManager.Instance.enableCheats = EnableCheats;
        ApplicationManager.Instance.skipTutorial = SkipTutorial;
        ApplicationManager.Instance.StartGame(DebugScene ? GameMode.Debug : GameMode.Normal, Difficulty.Hard);
    }
}
