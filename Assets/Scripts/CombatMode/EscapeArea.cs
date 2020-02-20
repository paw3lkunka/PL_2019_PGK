using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EscapeArea : MonoBehaviour
{
    #region Variables



    #endregion

    #region MonoBehaviour

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameManager.Instance.ourCrew.IndexOf(collision.gameObject) != -1)
        {
            GameManager.Instance.OnLocationExitInvoke();
            SceneManager.LoadScene("MainMap");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, .2f);
    }

    #endregion

    #region Component

    

    #endregion
}
