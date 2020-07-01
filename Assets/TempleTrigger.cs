using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameplayManager.Instance.visitedShrinesIds.Count >= GameplayManager.Instance.shrinesToVisit
        && other.gameObject.layer == LayerMask.NameToLayer("PlayerCrew"))
        {
            Debug.Log("Our jourey is finally done!");
            //TODO do something
        }
    }
}
