using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFaithHack : MonoBehaviour
{
    #region Variables

    public float value;

    #endregion

    #region MonoBehaviour

    private void Update()
    {
        GameManager.Instance.Faith = value;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    #endregion

    #region Component



    #endregion
}
