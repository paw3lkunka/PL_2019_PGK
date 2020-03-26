using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InfoLogZone : MonoBehaviour
{
    #region Variables

#pragma warning disable
    [SerializeField] private string header;
    [TextArea(3, 10)]
    [SerializeField] private string text;

    [SerializeField] private bool autoTime = true;
    [SerializeField] private float timeToRead;
#pragma warning restore

    private bool firstTime = true;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        if(autoTime)
        {
            timeToRead = Mathf.Ceil(text.Length / 4.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<CultLeader>())
        {
            InfoLog.Instance.EnterInfoLogZone();

            if(firstTime)
            {
                InfoLog.Instance.ShowLogForSeconds(header, text, timeToRead);
                firstTime = false;
            }
            else
            {
                InfoLog.Instance.SetInfo(header, text);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<CultLeader>())
        {
            InfoLog.Instance.ExitInfoLogZone();
        }
    }

    #endregion

    #region Component



    #endregion
}
