using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            Vector3 vec = transform.position - FindObjectOfType<LocationCentre>().transform.position;
            vec.y = 0;
            LocationCentre.exitDirection = vec.normalized;
            GameplayManager.Instance.ExitLocation();
        }
    }
}
