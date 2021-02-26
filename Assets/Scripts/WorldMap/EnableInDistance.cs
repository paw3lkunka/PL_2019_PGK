using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableInDistance : MonoBehaviour
{
    private readonly float enableDistanceNear = 250.0f;
    private readonly float enableDistanceFar = 350.0f;
    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        var distance = Vector3.Distance(transform.position, WorldSceneManager.Instance.Leader.transform.position);
        var color = meshRenderer.material.color;

        color.a = Mathf.InverseLerp(enableDistanceFar, enableDistanceNear, distance);
        meshRenderer.material.color = color;
    }
}
