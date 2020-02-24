using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoLogZone : MonoBehaviour
{
    #region Variables

#pragma warning disable
    [SerializeField] private string header;
    [TextArea(3, 10)]
    [SerializeField] private string text;
#pragma warning restore

    private bool firstTime = true;

    #endregion

    #region MonoBehaviour

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<CultLeader>())
        {
            InfoLog.Instance.SetInfo(header, text);

            if(firstTime)
            {
                InfoLog.Instance.ShowLog();
                firstTime = false;
            }
        }
    }

    #endregion

    #region Component

    

    #endregion
}
