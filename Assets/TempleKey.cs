using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleKey : MonoBehaviour
{
    public uint index = 0;
    public Material activatedMat;
    public Material inactivatedMat;

    private void OnEnable()
    {
        var renderer = GetComponent<MeshRenderer>();

        if ((GameplayManager.Instance?.visitedShrinesIds.Count ?? 0) > index)
        {
            renderer.material = activatedMat;
        }
        else
        {
            renderer.material = inactivatedMat;
        }
    }
}
