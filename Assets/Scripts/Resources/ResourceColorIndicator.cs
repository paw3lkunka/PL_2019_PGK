using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SustainedResource))]
public class ResourceColorIndicator : MonoBehaviour
{
    public List<MeshRenderer> indicators;
    [ColorUsage(true, true)] public Color maxResourceColor;
    [ColorUsage(true, true)] public Color minResourceColor;

    private SustainedResource sustainedResource;

    private void Awake()
    {
        sustainedResource = GetComponent<SustainedResource>();
    }

    void Update()
    {
        foreach (var indicator in indicators)
        {
            indicator.material.SetColor("_EmissionColor", Color.Lerp(minResourceColor, maxResourceColor, sustainedResource.ResourceAmount / sustainedResource.StartResourceAmount));
        }
    }
}
