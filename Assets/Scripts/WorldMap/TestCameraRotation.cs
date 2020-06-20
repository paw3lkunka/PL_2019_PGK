using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraRotation : MonoBehaviour
{
    public float rotationSpeed = 1.0f;

    private float lastCursorLocationX;
    private float angle = 0.0f;
    private bool shouldCameraMove = false;

    void Update()
    {
        if (shouldCameraMove)
        {
            float mouseXdelta = lastCursorLocationX - (Input.mousePosition.x + Screen.width / 2.0f) / Screen.width;

            angle = Mathf.LerpUnclamped(angle, angle + rotationSpeed, mouseXdelta);

            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
        }

        lastCursorLocationX = (Input.mousePosition.x + Screen.width / 2.0f) / Screen.width;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            shouldCameraMove = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            shouldCameraMove = false;
        }
    }
}
