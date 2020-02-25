using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class InfoLog : MonoBehaviour
{
    #region Variables

    public static InfoLog Instance { get; private set; }

    private Image background;
    private TextMeshProUGUI header;
    private TextMeshProUGUI text;

    private TextMeshProUGUI zoneIndicator;

    private bool isShown;
    private bool isLocked;
    private bool isInInfoZone;

    private float secondsTillHide;

    private NewInput input;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        Instance = this;

        background = GetComponent<Image>();
        header = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        zoneIndicator = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        input = GameManager.Instance.input;

        HideLog();
    }

    private void Start()
    {
        zoneIndicator.CrossFadeAlpha(0.0f, 0.05f, false);
    }

    private void OnEnable()
    {
        input.UI.ShowHideInfoLog.performed += ShowHideInputHandler;
        input.UI.ShowHideInfoLog.Enable();
    }

    private void OnDisable()
    {
        input.UI.ShowHideInfoLog.performed -= ShowHideInputHandler;
        input.UI.ShowHideInfoLog.Disable();
    }

    private void Update()
    {
        if(isShown && !isLocked)
        {
            secondsTillHide -= Time.deltaTime;

            if (secondsTillHide <= 0)
            {
                HideLog();
            }
        }
    }

    #endregion

    #region Component

    public void ShowLog()
    {
        isShown = true;
        background.CrossFadeAlpha(1.0f, 0.5f, false);
        header.CrossFadeAlpha(1.0f, 0.5f, false);
        text.CrossFadeAlpha(1.0f, 0.5f, false);
        
        if(isInInfoZone)
        {
            zoneIndicator.CrossFadeAlpha(0.0f, 0.5f, false);
        }
    }

    public void HideLog()
    {
        isShown = false;
        background.CrossFadeAlpha(0.0f, 0.5f, false);
        header.CrossFadeAlpha(0.0f, 0.5f, false);
        text.CrossFadeAlpha(0.0f, 0.5f, false);

        if (isInInfoZone)
        {
            zoneIndicator.CrossFadeAlpha(1.0f, 0.5f, false);
        }
    }

    public void SetInfo(string newHeader, string newText)
    {
        header.text = newHeader;
        text.text = newText;
    }

    public void ShowLogForSeconds(string newHeader, string newText, float seconds)
    {
        SetInfo(newHeader, newText);
        secondsTillHide = seconds;
        ShowLog();
    }

    public void EnterInfoLogZone()
    {
        isInInfoZone = true;

        if(!isShown)
        {
            zoneIndicator.CrossFadeAlpha(1.0f, 0.5f, false);
        }
    }

    public void ExitInfoLogZone()
    {
        isInInfoZone = false;

        if(!isShown)
        {
            zoneIndicator.CrossFadeAlpha(0.0f, 0.5f, false);
        }
    }

    #endregion

    #region Input

    private void ShowHideInputHandler(InputAction.CallbackContext ctx)
    {
        switch(isShown)
        {
            case true:
                HideLog();
                isLocked = false;
                break;

            case false:
                ShowLog();
                isLocked = true;
                break;
        }
    }

    #endregion
}
