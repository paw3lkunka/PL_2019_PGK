using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadStartScene : MonoBehaviour
{
    public void StartScene()
    {
        SceneManager.LoadScene(ApplicationManager.Instance.tutorialScene);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartScene();
        }
    }
}
