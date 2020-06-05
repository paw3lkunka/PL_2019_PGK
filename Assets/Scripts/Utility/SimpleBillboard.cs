using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBillboard : MonoBehaviour
{
    [System.Flags] public enum Axis 
    {
        None = 0,
        X = 1,
        Y = 2,
        Z = 4
    }

    public Axis lockAxis;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        Vector3 targetRotation = mainCam.transform.rotation.eulerAngles;
        if ((lockAxis & Axis.X) != 0)
        {
            targetRotation.x = 0;
        }
        if ((lockAxis & Axis.Y) != 0)
        {
            targetRotation.y = 0;
        }
        if ((lockAxis & Axis.Z) != 0)
        {
            targetRotation.z = 0;
        }
        transform.rotation = Quaternion.Euler(targetRotation);
    }
}
