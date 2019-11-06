using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButtonScript : MonoBehaviour
{
    public void ExitOnClick()
    {
        GameManager.Instance.Exit();
    }
}
