using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class Location : MonoBehaviour
{
    #region Variables

    public int generationID;

    public string sceneName;
    public Vector2 returnPoint;

    public Vector2 GlobalReturnPoint => (Vector2)transform.position + returnPoint;

    #endregion

    #region MonoBehaviour

    private void OnValidate()
    {
        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.savedPosition = GlobalReturnPoint;
        SceneManager.LoadScene(sceneName);
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
