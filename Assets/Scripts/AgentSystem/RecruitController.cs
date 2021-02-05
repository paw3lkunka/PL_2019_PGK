using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitController : MonoBehaviour
{
    public float fadeSpeed = 0.05f;
    public float targetAlpha = 0.5f;
    public MeshRenderer[] meshesToFade;
    private Collider triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        if (LocationManager.Instance.sceneMode == LocationMode.Neutral)
        {
            foreach (var meshRenderer in meshesToFade)
            {
                meshRenderer.enabled = true;
                Color targetColor = meshRenderer.material.color;
                targetColor.a = 0.0f;
                meshRenderer.material.color = targetColor;
            }
        }
        else
        {
            triggerCollider.enabled = false;
            foreach (var meshRenderer in meshesToFade)
            {
                meshRenderer.enabled = false;
            }
        }
    }

    private void Update()
    {
        if (LocationManager.Instance.sceneMode == LocationMode.Neutral)
        {
            foreach (var meshRenderer in meshesToFade)
            {
                meshRenderer.enabled = true;
            }

            if (RhythmMechanics.Instance && RhythmMechanics.Instance.Combo > 0)
            {
                triggerCollider.enabled = true;
                foreach (var meshRenderer in meshesToFade)
                {
                    Color targetColor = meshRenderer.material.color;
                    targetColor.a = targetAlpha;
                    meshRenderer.material.color = Color.Lerp(meshRenderer.material.color, targetColor, fadeSpeed);
                }
            }
            else
            {
                triggerCollider.enabled = false;
                foreach (var meshRenderer in meshesToFade)
                {
                    Color targetColor = meshRenderer.material.color;
                    targetColor.a = 0.0f;
                    meshRenderer.material.color = Color.Lerp(meshRenderer.material.color, targetColor, fadeSpeed);
                }
            }
        }
        
    }
}
