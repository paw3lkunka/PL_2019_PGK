using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            GameplayManager.Instance.MarkLastLocationAsVisitedShrine();
        }
    }
}
