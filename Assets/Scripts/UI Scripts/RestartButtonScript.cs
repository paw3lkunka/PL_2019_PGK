using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartButtonScript : MonoBehaviour
{
    public void RestartOnClick()
    {
        GameManager.Instance.Restart();
    }
}
