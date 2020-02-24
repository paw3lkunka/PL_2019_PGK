using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    #region Component

    public void ResumeOnClick()
    {
        Time.timeScale = 0;
        AudioTimeline.Instance.Resume();
        Destroy(gameObject);
    }

    public void ExitOnClick()
    {
        GameManager.Instance.Exit();
    }

    #endregion
}
