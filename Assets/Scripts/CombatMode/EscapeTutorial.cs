using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeTutorial : EscapeArea
{
    #region Variables

    #endregion

    #region MonoBehaviour

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<CultLeader>())
        {
            base.OnTriggerStay2D(collision);
            GameManager.Instance.RemoveCultistsFromCrew();
            GameManager.Instance.RestartCultists();
        }  
    }
    #endregion

    #region Component


    #endregion
}
