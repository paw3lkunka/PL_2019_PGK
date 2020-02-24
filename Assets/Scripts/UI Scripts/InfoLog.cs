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

#pragma warning disable
    [SerializeField] private float secondsForShow = 10.0f;
#pragma warning restore

    private Image background;
    private TextMeshProUGUI header;
    private TextMeshProUGUI text;

    private bool isShown;
    private bool isLocked = true;

    private float secondsTillHide;

    private NewInput input;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        Instance = this;

        secondsTillHide = secondsForShow;

        background = GetComponent<Image>();
        header = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text = transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        input = GameManager.Instance.input;
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
        if(isLocked == false)
        {
            secondsTillHide -= Time.deltaTime;

            if(secondsTillHide <= 0)
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

        secondsTillHide = secondsForShow;
    }

    public void HideLog()
    {
        isShown = false;
        background.CrossFadeAlpha(0.0f, 0.5f, false);
        header.CrossFadeAlpha(0.0f, 0.5f, false);
        text.CrossFadeAlpha(0.0f, 0.5f, false);
    }

    public void SetInfo(string newHeader, string newText)
    {
        header.text = newHeader;
        text.text = newText;
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
