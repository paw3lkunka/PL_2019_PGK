using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LoadStartScene : MonoBehaviour
{
    public void StartScene()
    {
        SceneManager.LoadScene(ApplicationManager.Instance.tutorialScene);
    }

    public void Update()
    {
        if (Keyboard.current[Key.Space].wasPressedThisFrame)
        {
            StartScene();
        }

        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            StartScene();
        }
    }
}
