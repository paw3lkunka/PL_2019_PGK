using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class LoadSceneOnAnyKey : MonoBehaviour
{
    public int sceneIndex = 0;

    public void StartScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            StartScene();
        }

        if (Gamepad.current != null && Gamepad.current.allControls.Any(x => x is ButtonControl button && x.IsPressed() && !x.synthetic))
        {
            StartScene();
        }
    }
}
