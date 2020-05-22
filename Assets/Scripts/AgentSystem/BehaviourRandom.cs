﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Moveable))]
public class BehaviourRandom : MonoBehaviour, IBehaviour
{
#pragma warning disable
    [SerializeField] private Vector2 targetChangeInterval;
    [SerializeField] private float maxRange = 10.0f;
#pragma warning restore
    private Moveable moveable;

    #region MonoBehaviour

    private void Awake()
    {
        moveable = GetComponent<Moveable>();
    }

    private void OnEnable()
    {
        StartCoroutine(ChangeTargetRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    #endregion

    private IEnumerator ChangeTargetRoutine()
    {
        while(true)
        {
            UpdateTarget(transform.position + new Vector3(Random.Range(0.0f, maxRange), 0.0f, Random.Range(0.0f, maxRange)));
            yield return new WaitForSeconds(Random.Range(targetChangeInterval.x, targetChangeInterval.y));
        }
    }

    public void UpdateTarget(Vector3? target)
    {
        moveable.Go((Vector3)target);
    }
}
