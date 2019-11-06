using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(RestartOnClick);
    }

    void RestartOnClick()
    {
        GameManager.Instance.Restart();
    }
}
