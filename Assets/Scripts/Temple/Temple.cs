using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Temple : InfoLogZone
{
    #region Variables

#pragma warning disable
    public GameObject[] toEnable;
    public GameObject[] toDisable;
#pragma warning restore

    #endregion

    #region MonoBehaviour

    protected new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CultLeader>())
        {
            InfoLog.Instance.EnterInfoLogZone();
            InfoLogInvokerExtensions.SetInfoLog(this);

            foreach (var item in toEnable)
            {
                item.SetActive(true);
            }

            foreach (var item in toDisable)
            {
                item.SetActive(false);
            }
        }
    }

    #endregion
}