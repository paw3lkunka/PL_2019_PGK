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

        GameManager.Instance.StartCoroutine(Routine());

        IEnumerator Routine()
        {
            foreach (var obj in GameManager.Instance.mainMapScene.GetRootGameObjects())
            {
                obj.SetActive(false);
            }
            AsyncOperation load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            yield return new WaitUntil(()=>load.isDone);
            GameManager.Instance.locationScene = SceneManager.GetSceneByName(sceneName);
        }
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
