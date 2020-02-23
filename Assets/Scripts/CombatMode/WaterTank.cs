using System;
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

            try
            {
                GetComponent<DynamicObject>().DestroyAndRemember();
            }
            catch(NullReferenceException)
            {
                Destroy(gameObject);
            }
        }
    }

    #endregion

    #region Component



    #endregion
}
