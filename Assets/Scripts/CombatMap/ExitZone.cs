using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZone : MonoBehaviour
{
    public LocationCentre center;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            Vector3 vec = transform.position - center.transform.position;
            vec.y = 0;
            LocationCentre.exitDirection = vec.normalized;
            GameplayManager.Instance.ExitLocation();
        }
    }
}
