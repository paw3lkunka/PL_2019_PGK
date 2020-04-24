using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    public bool SkipTutorial { get; set; }
    public bool EnableCheats { get; set; }
    public bool DebugScene { get; set; }

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
