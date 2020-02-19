using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CombatArea : MonoBehaviour
{
    #region Variables

    public string sceneName;
    public Vector2 returnPoint;

    public Vector2 GlobalReturnPoint => (Vector2)transform.position + returnPoint;

    #endregion

    #region MonoBehaviour

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.savedPosition = GlobalReturnPoint;
        SceneManager.LoadSceneAsync(sceneName);
        GameManager.Instance.OnLocationEnterInvoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(GlobalReturnPoint, .2f);
    }

    #endregion

    #region Component



    #endregion
}
