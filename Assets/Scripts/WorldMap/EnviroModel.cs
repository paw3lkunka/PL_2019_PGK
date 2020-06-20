using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class EnviroModel : EnviroObject
{
    private new MeshRenderer renderer;
    private MeshFilter mesh;

    public Mesh[] meshes = new Mesh[0];
    public Material[] materials = new Material[0];

    protected void Awake() => Initialize();
    protected void OnValidate() => Initialize();

    protected void Initialize()
    {
        renderer = GetComponent<MeshRenderer>();
        mesh = GetComponent<MeshFilter>();
    }

    [ContextMenu("Randomize")]
    public override void Randomize()
    {
        if (renderer == null || mesh == null)
        {
            Initialize();
        }

        int index = Random.Range(0, meshes.Length);
        mesh.sharedMesh = meshes[index];
        renderer.material = index < materials.Length ? materials[index] : materials.Last();
    }
}
