using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InfoLogZone : MonoBehaviour, IInfoLogInvoker
{
    #region Variables

#pragma warning disable
    [SerializeField] private string header;
    [TextArea(3, 10)]
    [SerializeField] private string mainText;
    [TextArea(3, 10)]
    [SerializeField] private string keyboardText;
    [TextArea(3, 10)]
    [SerializeField] private string gamepadText;

    [SerializeField] private bool autoTime = true;
    [SerializeField] private float timeToRead;
#pragma warning restore


    private string finalText;
    private bool firstTime = true;

    #endregion

    #region MonoBehaviour

    protected void Awake()
    {
        if(autoTime)
        {
            timeToRead = Mathf.Ceil((mainText.Length + keyboardText.Length + gamepadText.Length) / 4.0f);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<CultLeader3d>())
        {
            InfoLog.Instance.EnterInfoLogZone();
            InfoLogInvokerExtensions.SetInfoLog(this);
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<CultLeader3d>())
        {
            InfoLog.Instance.ExitInfoLogZone();
        }
    }

    #endregion

    #region IInfoLogInvoker

    public void SetInfoLog()
    {
        UpdateFinalText();

        if (firstTime)
        {
            InfoLog.Instance.ShowLogForSeconds(finalText, header, timeToRead);
            firstTime = false;
        }
        else
        {
            InfoLog.Instance.SetInfo(finalText, header);
        }
    }

    public void UpdateInfoLog()
    {
        UpdateFinalText();
        InfoLog.Instance.ShowLogForSeconds(finalText, header, timeToRead);
    }

    #endregion

    #region Component

    protected void UpdateFinalText()
    {
        finalText = mainText;
        if (finalText != "")
        {
            finalText += "\n";
        }

        switch (ApplicationManager.Instance.CurrentInputScheme)
        {
            case InputSchemeEnum.MouseKeyboard:
                finalText += keyboardText;
                break;

            case InputSchemeEnum.Gamepad:
                finalText += gamepadText;
                break;

            case InputSchemeEnum.JoystickKeyboard:
                finalText += keyboardText;
                break;
        }
    }

    #endregion
}
