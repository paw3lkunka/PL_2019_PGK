using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSource : MonoBehaviour
{
    #region Variables

#pragma warning disable
    [SerializeField]
    private float refillAmout = 0.3f;
#pragma warning restore

    #endregion

    #region MonoBehaviour

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.ourCrew.IndexOf(collision.gameObject) != -1)
        {
            GameManager.Instance.Water = Mathf.Max(GameManager.Instance.Water, refillAmout);
        }
    }

    #endregion

    #region Component



    #endregion
}
