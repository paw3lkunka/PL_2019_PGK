using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DynamicObject))]
public class WaterTank : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private float capacity = 0.1f;

    #endregion

    #region MonoBehaviour

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.ourCrew.IndexOf(collision.gameObject) != -1)
        {
            GameManager.Instance.Water += capacity;
            capacity = 0; //for sure

            GetComponent<DynamicObject>().DestroyAndRemember();
        }
    }

    #endregion

    #region Component



    #endregion
}
