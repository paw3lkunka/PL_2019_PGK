using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToShootDirection : MonoBehaviour
{
    public float rotationSpeed = 0.01f;

    void Update()
    {
        Vector3 lookDirection = CombatCursorManager.Instance.shootDirection.normalized;
        if (lookDirection.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), rotationSpeed);
        }
    }
}
