using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDepleter : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private float veloctyThreshold = 0.01f;
    [field: SerializeField, GUIName("WaterDepletionRate")]
    public float WaterDepletionRate { get; private set; } = 0.1f;
    [field: SerializeField, GUIName("FaithDepletionRate")] 
    public float FaithDepletionRate { get; private set; } = 0.05f;
#pragma warning restore

    private Vector3 lastFramePos;

    private void FixedUpdate()
    {
        if ((lastFramePos - transform.position).magnitude > veloctyThreshold)
        {
            GameplayManager.Instance.Water -= WaterDepletionRate;
            GameplayManager.Instance.Faith -= FaithDepletionRate * GameplayManager.Instance.cultistInfos.Count;
        }
        lastFramePos = transform.position;
    }
}
