using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMap : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.mainMapScene = SceneManager.GetActiveScene();
    }
}
