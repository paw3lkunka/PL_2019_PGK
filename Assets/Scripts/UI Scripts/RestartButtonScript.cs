using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartButtonScript : MonoBehaviour
{
    #region Variables



    #endregion

    #region MonoBehaviour

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.savedPosition = Vector2.zero);
    }

    #endregion

    #region Component

    public void RestartOnClick()
    {
        GameManager.Instance.Restart();
    }

    #endregion
}
