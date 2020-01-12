using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CombatArea : MonoBehaviour
{
    public string sceneName;
    public Vector2 returnPoint;

    public Vector2 GlobalReturnPoint => (Vector2)transform.position + returnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.savedPosition = GlobalReturnPoint;
        SceneManager.LoadScene(sceneName);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(GlobalReturnPoint, .2f);
    }
}
