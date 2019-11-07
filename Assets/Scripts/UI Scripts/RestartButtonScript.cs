using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartButtonScript : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.savedPosition = Vector2.zero);
    }

    public void RestartOnClick()
    {
        GameManager.Instance.Restart();
    }
}
