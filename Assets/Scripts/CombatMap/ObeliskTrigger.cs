using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObeliskTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            GameplayManager.Instance.MarkLastLocationAsVisitedObelisk();
        }
    }
}
