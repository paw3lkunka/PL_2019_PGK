using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainScene : MonoBehaviour
{
    public void MainScene()
    {
        SceneManager.LoadScene(ApplicationManager.Instance.worldMapScene);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MainScene();
        }
    }
}
