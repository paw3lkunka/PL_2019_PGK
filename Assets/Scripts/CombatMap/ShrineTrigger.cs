using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineTrigger : MonoBehaviour
{
    public Material inactiveMat;
    public Material activeMat;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.sharedMaterial = inactiveMat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            GameplayManager.Instance.MarkLastLocationAsVisitedShrine();
            GameplayManager.Instance.obeliskActivated = false;
            meshRenderer.sharedMaterial = activeMat;
        }
    }
}
