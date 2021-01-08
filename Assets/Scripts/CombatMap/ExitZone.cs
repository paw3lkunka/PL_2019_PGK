using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZone : MonoBehaviour
{
    public LocationEnterExitController center;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            center.isExiting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            center.isExiting = false;
        }
    }
}
