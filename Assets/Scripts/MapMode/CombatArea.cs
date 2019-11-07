using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CombatArea : MonoBehaviour
{
    public string sceneName;
    public Vector2 returnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.savedPosition = returnPoint;
        SceneManager.LoadScene(sceneName);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(returnPoint, .2f);
    }
}
