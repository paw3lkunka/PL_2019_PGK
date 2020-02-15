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
            GameManager.Instance.StartCoroutine(Routine());

            IEnumerator Routine()
            {
                var unload = SceneManager.UnloadSceneAsync(GameManager.Instance.locationScene);
                yield return new WaitUntil(()=>!unload.isDone);
                foreach (var obj in GameManager.Instance.mainMapScene.GetRootGameObjects())
                {
                    obj.SetActive(true);
                    if (obj.CompareTag("Player"))
                    {
                        obj.transform.position = GameManager.Instance.savedPosition;
                        obj.GetComponent<MoveToCursorClick>().ResetTarget();
                    }
                }
            }

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
