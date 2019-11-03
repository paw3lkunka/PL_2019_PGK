using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EscapeArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( GameManager.Instance.ourCrew.IndexOf(collision.gameObject) != -1 )
        {
            Debug.Log("Escape");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, .2f);
    }
}
