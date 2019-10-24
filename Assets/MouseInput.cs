using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseInput : MonoBehaviour
{
    public UnityEvent OnLeftButton, OnRigthButton;

    private void Update()
    {
        if( Input.GetMouseButtonDown(0) )
        {
            OnLeftButton.Invoke();
        }

        if (Input.GetMouseButtonDown(1))
        {
            OnRigthButton.Invoke();
        }
    }

}
