using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldMapCamera : MonoBehaviour
{
    public Camera mapCamera;
    public Transform closestPoint;
    public Transform middlePoint;
    public Transform furthestPoint;

    public float rotationSpeed = 1.0f;
    public float smoothing = 0.1f;

    public float mousePointerSensitivity = 5.0f;
    public float mouseScrollSensitivity = 3.0f;
    public float joystickSensivity = 20.0f;
    public float bumpersSensitivity = 0.4f;

    private float angle = 0.0f;
    private bool isRotating = false;

    private float zoom = 0.5f;
    private bool isZooming = false;
    private float zoomHold = 0.0f;

    void Awake()
    {
        angle = GetComponent<Transform>().localRotation.eulerAngles.y;
    }

    private void OnEnable()
    {
        ApplicationManager.Instance.Input.Gameplay.RotateCamera.performed += RotationPerformed;
        ApplicationManager.Instance.Input.Gameplay.RotateCamera.canceled += RotationCanceled;
        ApplicationManager.Instance.Input.Gameplay.ZoomCamera.performed += ZoomPerformed;
        ApplicationManager.Instance.Input.Gameplay.ZoomCamera.canceled += ZoomCanceled;
        ApplicationManager.Instance.Input.Gameplay.ZoomCamera.Enable();
        ApplicationManager.Instance.Input.Gameplay.RotateCamera.Enable();
    }

    private void OnDisable()
    {
        ApplicationManager.Instance.Input.Gameplay.RotateCamera.performed -= RotationPerformed;
        ApplicationManager.Instance.Input.Gameplay.RotateCamera.canceled -= RotationCanceled;
        ApplicationManager.Instance.Input.Gameplay.ZoomCamera.performed -= ZoomPerformed;
        ApplicationManager.Instance.Input.Gameplay.ZoomCamera.canceled -= ZoomCanceled;
        ApplicationManager.Instance.Input.Gameplay.ZoomCamera.Disable();
        ApplicationManager.Instance.Input.Gameplay.RotateCamera.Disable();
    }

    private void RotationPerformed(InputAction.CallbackContext obj)
    {
        isRotating = true;
    }

    private void RotationCanceled(InputAction.CallbackContext obj)
    {
        isRotating = false;
    }

    private void ZoomPerformed(InputAction.CallbackContext obj)
    {
        isZooming = true;
        switch (ApplicationManager.Instance.CurrentInputScheme)
        {
            case InputSchemeEnum.Gamepad:
                zoomHold = obj.ReadValue<float>();
                break;
            
            case InputSchemeEnum.JoystickKeyboard:
                break;

            case InputSchemeEnum.Touchscreen:
                break;
        }
    }

    private void ZoomCanceled(InputAction.CallbackContext obj)
    {
        isZooming = false;
        zoomHold = 0.0f;
    }

    void Update()
    {
        Vector3 targetPosition; Quaternion targetRotation;

        if(isZooming)
        {
            float zoomDelta = Time.deltaTime;

            switch (ApplicationManager.Instance.CurrentInputScheme)
            {
                case InputSchemeEnum.MouseKeyboard:
                    zoomDelta *= Mouse.current.scroll.ReadValue().y * mouseScrollSensitivity;
                    break;

                case InputSchemeEnum.Gamepad:
                    zoomDelta *= zoomHold * bumpersSensitivity;
                    break;
                
                case InputSchemeEnum.JoystickKeyboard:
                    break;

                case InputSchemeEnum.Touchscreen:
                    break;
            }
            
            zoom = Mathf.Clamp01(zoom - zoomDelta);
        }

        if (zoom < 0.5f)
        {
            targetPosition = Vector3.Lerp(closestPoint.position, middlePoint.position, zoom * 2.0f);
            targetRotation = Quaternion.Lerp(closestPoint.rotation, middlePoint.rotation, zoom * 2.0f);
        }
        else
        {
            targetPosition = Vector3.Lerp(middlePoint.position, furthestPoint.position, (zoom - 0.5f) * 2.0f);
            targetRotation = Quaternion.Lerp(middlePoint.rotation, furthestPoint.rotation, (zoom - 0.5f) * 2.0f);
        }

        if (isRotating)
        {
            float rotationDelta = Time.deltaTime;

            switch (ApplicationManager.Instance.CurrentInputScheme)
            {
                case InputSchemeEnum.MouseKeyboard:
                    rotationDelta *= Mouse.current.delta.ReadValue().x * mousePointerSensitivity;
                    break;

                case InputSchemeEnum.Gamepad:
                    rotationDelta *= Gamepad.current.rightStick.ReadValue().x * joystickSensivity;
                    break;

                case InputSchemeEnum.JoystickKeyboard:
                    break;

                case InputSchemeEnum.Touchscreen:
                    break;
            }

            angle = Mathf.LerpUnclamped(angle, angle + rotationDelta, rotationSpeed);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
        }

        mapCamera.transform.position = Vector3.Lerp(mapCamera.transform.position, targetPosition, smoothing);
        mapCamera.transform.rotation = Quaternion.Lerp(mapCamera.transform.rotation, targetRotation, smoothing);
    }
}
