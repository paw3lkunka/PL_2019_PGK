﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDepleter : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private float veloctyThreshold = 0.01f;
    [SerializeField] private float waterDepletionRate = 0.1f;
    [SerializeField] private float faithDepletionRate = 0.05f;
#pragma warning restore

    private Vector3 lastFramePos;

    private void FixedUpdate()
    {
        if ((lastFramePos - transform.position).magnitude > veloctyThreshold)
        {
            GameplayManager.Instance.Water -= waterDepletionRate;
            GameplayManager.Instance.Faith -= faithDepletionRate * GameplayManager.Instance.ourCrew.Count;
        }
        lastFramePos = transform.position;
    }
}
