using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZone : MonoBehaviour
{
    static public float angle = 180;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            Vector3 vec = transform.position - FindObjectOfType<LocationCentre>().transform.position;
            vec.y = 0;
            angle = Vector3.Angle(Vector3.forward, vec.normalized);
            GameplayManager.Instance.ExitLocation();
        }
    }
}
