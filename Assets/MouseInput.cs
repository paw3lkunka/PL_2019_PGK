using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    public UnityEvent OnLeftButton, OnRigthButton;

    public GameObject walkTargetIndicator;
    public GameObject shootTargetIndicator;

    public void PlaceWalkTargetIndicator() => walkTargetIndicator.transform.position = GameManager.Instance.MousePos;
    public void PlaceShootTargetIndicator() => shootTargetIndicator.transform.position = GameManager.Instance.MousePos;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnLeftButton.Invoke();
        }

        if (Input.GetMouseButtonDown(1))
        {
            OnRigthButton.Invoke();
        }
    }

    private static MouseInput instance;

    public static MouseInput Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("MouseInput");
                return instance = obj.AddComponent<MouseInput>();
            }
            return instance;
        }
    }

    private void OnValidate()
    {
        instance = this;
    }
}
