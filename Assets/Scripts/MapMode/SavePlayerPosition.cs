using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePlayerPosition : MonoBehaviour
{
    void Start()
    {
        LoadPosition();
    }

    public void LoadPosition()
    {   
        transform.position = GameManager.Instance.savedPosition;
    }
}
