using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public Transform shield;

    private Vector3 oldDirection;

    // Start is called before the first frame update
    void Start()
    {
        oldDirection = Vector3.forward;
        shield.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraDirection = Camera.main.transform.forward;

        float angle = Vector3.SignedAngle(oldDirection, cameraDirection, Vector3.up);
        
        shield.Rotate(Vector3.forward, angle);

        oldDirection = cameraDirection;
    }
}
