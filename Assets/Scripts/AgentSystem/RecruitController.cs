using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitController : MonoBehaviour
{
    public Color projectorOnColor = Color.yellow;
    public float fadeSpeed = 0.05f;
    private Collider triggerCollider;
    private Projector projector;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();
        projector = GetComponentInChildren<Projector>();
    }

    private void Update()
    {
        if (LocationManager.Instance.sceneMode == LocationMode.Neutral)
        {
            projector.enabled = true;
            if (RhythmMechanics.Instance && RhythmMechanics.Instance.Combo > 0)
            {
                triggerCollider.enabled = true;
                projector.material.color = Color.Lerp(projector.material.color, projectorOnColor, fadeSpeed);
            }
            else
            {
                triggerCollider.enabled = false;
                projector.material.color = Color.Lerp(projector.material.color, Color.black, fadeSpeed);
            }
        }
        else
        {
            triggerCollider.enabled = false;
            projector.enabled = false;
        }
    }
}
