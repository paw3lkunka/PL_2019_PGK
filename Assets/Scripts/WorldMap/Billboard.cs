using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public bool X = false;
    public bool Y = true;
    public bool Z = false;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        Vector3 camRotation = mainCam.transform.rotation.eulerAngles;
        Vector3 thisRotation = transform.rotation.eulerAngles;
        if (!X)
        {
            camRotation.x = thisRotation.x;
        }
        if (!Y)
        {
            camRotation.y = thisRotation.y;
        }
        if (!Z)
        {
            camRotation.z = thisRotation.z;
        }

        transform.rotation = Quaternion.Euler(camRotation);
    }
}
