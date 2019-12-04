using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaterTank : MonoBehaviour
{
    [SerializeField]
    private float capacity = 0.1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( GameManager.Instance.ourCrew.IndexOf(collision.gameObject) != -1 )
        {
            GameManager.Instance.Water += capacity;
            capacity = 0; //for sure
            Destroy(gameObject);
        }
    }
}
