using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class EnviroSprite : EnviroObject
{
    private new MeshRenderer renderer;

    public Material[] materials = new Material[0];

    protected void Awake() => Initialize();
    protected void OnValidate() => Initialize();

    protected void Initialize()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    [ContextMenu("Randomize")]
    public override void Randomize()
    {
        if (renderer == null)
        {
            Initialize();
        }
        renderer.material = materials[Random.Range(0, materials.Length)];
    }
}
