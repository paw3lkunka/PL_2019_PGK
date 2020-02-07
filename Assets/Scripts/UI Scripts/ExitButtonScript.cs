using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButtonScript : MonoBehaviour
{
    #region Variables



    #endregion

    #region MonoBehaviour



    #endregion

    #region Component

    public void ExitOnClick()
    {
        GameManager.Instance.Exit();
    }

    #endregion
}
