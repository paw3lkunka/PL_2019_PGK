using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            LocationManager.Instance.isExiting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            LocationManager.Instance.isExiting = false;
        }
    }
}
