using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableInDistance : MonoBehaviour
{
    private readonly float enableDistance = 250.0f;
    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, WorldSceneManager.Instance.Leader.transform.position) < enableDistance)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
}
