using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDepleter : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private bool isTimeBased = false;
    [SerializeField] private float veloctyThreshold = 0.01f;

    // These properties are left as not-auto to allow for easy modification for getters
    [SerializeField] private float _waterDepletionRate;
    public float WaterDepletionRate
    {
        get => _waterDepletionRate * GameplayManager.Instance.cultistInfos.Count;
        private set => _waterDepletionRate = value;
    }

    [SerializeField] private float _faithDepletionRate;
    public float FaithDepletionRate
    {
        get => _faithDepletionRate;
        private set => _faithDepletionRate = value;
    }
#pragma warning restore

    private Vector3 lastFramePos;

    private void FixedUpdate()
    {
        if (isTimeBased || (lastFramePos - transform.position).magnitude > veloctyThreshold)
        {
            GameplayManager.Instance.Water -= WaterDepletionRate;
            GameplayManager.Instance.Faith -= FaithDepletionRate;
        }
        lastFramePos = transform.position;
    }
}
