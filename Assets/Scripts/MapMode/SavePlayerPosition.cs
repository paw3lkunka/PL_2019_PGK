using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlayerPosition : MonoBehaviour
{
    #region Variables



    #endregion

    #region MonoBehaviour

    void Start()
    {
        LoadPosition();
    }

    #endregion

    #region Component

    public void LoadPosition()
    {
        transform.position = GameManager.Instance.savedPosition;
    }

    #endregion
}
