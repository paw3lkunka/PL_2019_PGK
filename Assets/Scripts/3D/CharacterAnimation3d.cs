using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Animates character based on movement and switches sprites accordingly
/// </summary>
/// <remarks>
/// Some possible ways to expand it in the future
/// - Move some responsibility to custom shader (uv scrolling or texture atlasing)
/// </remarks>
public class CharacterAnimation3d : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private float threshold = 0.05f;
    [Header("Debug materials")]
    [SerializeField] private bool useDebug = true;
    [SerializeField] private Material up;
    [SerializeField] private Material down;
    [SerializeField] private bool useNegativeScale = true;
    [SerializeField] private Material left;
    [SerializeField] private Material right;
#pragma warning restore

#region PrivateFields
    private Vector3 lastFramePos;
#endregion

    private void Start()
    {
        lastFramePos = transform.position;
    }

    private void Update()
    {
        Vector3 deltaPos = transform.position - lastFramePos;

        if (useDebug)
        {
            if (deltaPos.magnitude > threshold)
            {
                // Decide if the animation should be vertical or horizontal
                // Where the lequal condition is this side will be favoured if the vector values are equal
                if (Mathf.Abs(deltaPos.x) > Mathf.Abs(deltaPos.z))
                {
                    // Vertical movement
                    if (Mathf.Sign(deltaPos.x) > 0)
                    {
                        // Down
                        meshRenderer.sharedMaterial = down;
                    }
                    else
                    {
                        // Up
                        meshRenderer.sharedMaterial = up;
                    }
                }
                else
                {
                    // Horizontal movement
                    float scaleX = transform.localScale.x;
                    if (Mathf.Sign(deltaPos.z) > 0)
                    {
                        // Right
                        if (useNegativeScale)
                        {
                            meshRenderer.sharedMaterial = right;
                            if (Mathf.Sign(scaleX) > 0.0f)
                            {
                                scaleX *= -1.0f;
                            }
                            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
                        }
                        else
                        {
                            meshRenderer.sharedMaterial = right;
                        }
                    }
                    else
                    {
                        // Left
                        if (useNegativeScale)
                        {
                            meshRenderer.sharedMaterial = right;
                            if (Mathf.Sign(scaleX) < 0.0f)
                            {
                                scaleX *= -1.0f;
                            }
                            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
                        }
                        else
                        {
                            meshRenderer.sharedMaterial = left;
                        }
                    }
                }
            }
        }

        lastFramePos = transform.position;
    }
}
