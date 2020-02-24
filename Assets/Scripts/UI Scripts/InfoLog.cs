using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InfoLog : MonoBehaviour
{
    #region Variables

    public InfoLog Instance { get; private set; }

    private TextMeshProUGUI header;
    private TextMeshProUGUI text;

    private float secondsToShow;
    private bool isLocked;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    #region Component

    public void SetInfo(string header, string text)
    {

    }

    #endregion
}
