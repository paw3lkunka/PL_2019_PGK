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
public class CharacterAnimation : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private float threshold = 0.05f;
    [SerializeField] private int frames = 13;
    [SerializeField] private float animationSpeed = 1.0f;
    [Header("Animated materials")]
    [SerializeField] private float rotationOffset = 0.0f;
    [SerializeField] private Material up;
    [SerializeField] private Material down;
    [SerializeField] private bool useNegativeScale = true;
    [SerializeField] private Material left;
    [SerializeField] private Material right;
#pragma warning restore

#region PrivateFields
    private Vector3 lastFramePos;

    private float animationTime = 0.0f;
    private int currentFrame = 0;
    private float texScale;
#endregion

    private void Start()
    {
        lastFramePos = transform.position;
        texScale = 1.0f / frames;
        currentFrame = 0;
        up = new Material(up);
        down = new Material(down);
        left = new Material(left);
        if (right != null)
        {
            right = new Material(right);
        }
    }

    private void Update()
    {
        Vector3 deltaPos = transform.position - lastFramePos;
        deltaPos =  Quaternion.AngleAxis(rotationOffset, Vector3.up) * Quaternion.Inverse(transform.parent.rotation) * deltaPos;
        float magnitude = deltaPos.magnitude;
        if (magnitude > threshold)
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

            // Animation
            animationTime += magnitude * animationSpeed;
            currentFrame = Mathf.FloorToInt(animationTime) % frames;
        }
        else
        {
            animationTime = 0;
            currentFrame = 0;
        }

        meshRenderer.sharedMaterial.SetTextureScale("_MainTex", new Vector2(texScale, 1.0f));
        meshRenderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2((float)currentFrame / frames, 0.0f));

        lastFramePos = transform.position;
    }
}
