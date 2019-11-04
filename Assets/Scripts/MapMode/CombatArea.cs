using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CombatArea : MonoBehaviour
{
    public int sceneIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
