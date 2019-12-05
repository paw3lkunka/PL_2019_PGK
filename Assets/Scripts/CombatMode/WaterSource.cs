using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSource : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance.ourCrew.IndexOf(collision.gameObject) != -1)
        {
            GameManager.Instance.Water = Mathf.Max(GameManager.Instance.Water, 0.8f);
        }
    }
}
