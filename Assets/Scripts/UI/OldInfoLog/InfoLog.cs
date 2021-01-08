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
    [SerializeField] private bool isShown = true;
    [SerializeField] private bool isLocked = true;
    [SerializeField] private GameObject zoneIndicator;
#pragma warning restore

    private Image background;
    private TextMeshProUGUI header;
    private TextMeshProUGUI text;

    private bool isInInfoZone;
    [HideInInspector] public IInfoLogInvoker LastInvoker { get; set; }

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

        input = ApplicationManager.Instance.Input;

        HideLog();
    }

    private void Start()
    {
        zoneIndicator.SetActive(false);

        if(isShown)
        {
            ShowLog();
        }
        else
        {
            HideLog();
        }
    }

    private void OnEnable()
    {
        ApplicationManager.Instance.InputSchemeChange += OnInputSchemeChange;

        input.Gameplay.Interact.performed += ShowHideInputHandler;
        input.Gameplay.Interact.Enable();
    }

    private void OnDisable()
    {
        ApplicationManager.Instance.InputSchemeChange -= OnInputSchemeChange;

        input.Gameplay.Interact.performed -= ShowHideInputHandler;
        input.Gameplay.Interact.Disable();
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
            zoneIndicator.SetActive(false);
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
            zoneIndicator.SetActive(true);
        }
    }

    public void SetInfo(string newText, string newHeader = "")
    {
        if (newHeader == "")
        {
            header.gameObject.SetActive(false);
        }
        else
        {
            header.gameObject.SetActive(true);
            header.text = newHeader;
        }
        text.text = newText;
    }

    public void ShowLogForSeconds(string newText, string newHeader, float seconds)
    {
        SetInfo(newText, newHeader);
        secondsTillHide = seconds;
        ShowLog();
    }

    public void EnterInfoLogZone()
    {
        isInInfoZone = true;

        if(!isShown)
        {
            zoneIndicator.SetActive(true);
        }
    }

    public void ExitInfoLogZone()
    {
        isInInfoZone = false;

        if(!isShown)
        {
            zoneIndicator.SetActive(false);
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

    private void OnInputSchemeChange(PlayerInput obj)
    {
        if(LastInvoker as Object)
        {
            LastInvoker.UpdateInfoLog();
        }
    }

    #endregion
}
