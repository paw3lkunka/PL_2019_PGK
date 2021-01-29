using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public Transform shield;
    private Transform cameraPivot;

    private void Start()
    {
        cameraPivot = Camera.main.GetComponentInParent<Transform>();
    }

    void Update()
    {
        shield.localRotation = Quaternion.Euler(0.0f, 0.0f, cameraPivot.rotation.eulerAngles.y);
    }
}
