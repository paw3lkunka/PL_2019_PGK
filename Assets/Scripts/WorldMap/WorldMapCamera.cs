using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldMapCamera : MonoBehaviour
{
    public new Camera camera;
    public Transform closestPoint;
    public Transform middlePoint;
    public Transform furthestPoint;

    public float rotationSpeed = 1.0f;
    public float zoomSmoothing = 0.1f;
    public float zoomSpeed = 0.1f;

    private float lastCursorLocationX;
    private float angle = 0.0f;
    private bool shouldCameraMove = false;
    private float zoom = 0.5f;

    void Awake()
    {
        angle = GetComponent<Transform>().localRotation.eulerAngles.y;
    }

    private void OnEnable()
    {
        ApplicationManager.Instance.Input.Gameplay.RotateCamera.performed += CameraRotationStart;
        ApplicationManager.Instance.Input.Gameplay.RotateCamera.canceled += CameraRotationCancel;
        ApplicationManager.Instance.Input.Gameplay.ZoomCamera.performed += CameraZoom;
        ApplicationManager.Instance.Input.Gameplay.ZoomCamera.Enable();
        ApplicationManager.Instance.Input.Gameplay.RotateCamera.Enable();
    }

    private void OnDisable()
    {
        ApplicationManager.Instance.Input.Gameplay.RotateCamera.performed -= CameraRotationStart;
        ApplicationManager.Instance.Input.Gameplay.RotateCamera.canceled -= CameraRotationCancel;
        ApplicationManager.Instance.Input.Gameplay.ZoomCamera.performed -= CameraZoom;
        ApplicationManager.Instance.Input.Gameplay.ZoomCamera.Disable();
        ApplicationManager.Instance.Input.Gameplay.RotateCamera.Disable();
    }

    private void CameraRotationCancel(InputAction.CallbackContext obj)
    {
        shouldCameraMove = false;
    }

    private void CameraRotationStart(InputAction.CallbackContext obj)
    {
        shouldCameraMove = true;
    }

    private void CameraZoom(InputAction.CallbackContext obj)
    {
        zoom -= zoomSpeed * obj.ReadValue<Vector2>().y * 0.1f;
        zoom = Mathf.Clamp01(zoom);
    }

    void Update()
    {
        Vector3 targetPosition;
        Quaternion targetRotation;

        if (zoom < 0.5f)
        {
            targetPosition = Vector3.Lerp(closestPoint.position, middlePoint.position, zoom * 2.0f);
            targetRotation = Quaternion.Slerp(closestPoint.rotation, middlePoint.rotation, zoom * 2.0f);
        }
        else
        {
            targetPosition = Vector3.Lerp(middlePoint.position, furthestPoint.position, (zoom - 0.5f) * 2.0f);
            targetRotation = Quaternion.Slerp(middlePoint.rotation, furthestPoint.rotation, (zoom - 0.5f) * 2.0f);
        }

        if (shouldCameraMove)
        {
            float mouseXdelta = lastCursorLocationX - (Input.mousePosition.x + Screen.width / 2.0f) / Screen.width;

            angle = Mathf.LerpUnclamped(angle, angle * rotationSpeed, mouseXdelta);

            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
        }

        camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition, zoomSmoothing);
        camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, targetRotation, zoomSmoothing);

        lastCursorLocationX = (Input.mousePosition.x + Screen.width / 2.0f) / Screen.width;
    }
}
