using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDepleter : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private bool isTimeBased = false;
    [SerializeField] private bool shouldDepleteHealth = false;
    [SerializeField] private float veloctyThreshold = 0.01f;
    [SerializeField] private float waterHealthLosingThreshold = 0.01f;

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
        get  
        {
            float finalValue =_faithDepletionRate * GameplayManager.Instance.CurrentFaithMultiplier;
            if(GameplayManager.Instance.IsAvoidingFight)
            {
                finalValue *= 1.0f + GameplayManager.Instance.avoidingFightsFaithDebuf;
            }
            return finalValue;
        }
        private set => _faithDepletionRate = value;
    }

    [SerializeField] private float _healthDepletionRate;
    public float HealthDepletionRate
    {
        get => _healthDepletionRate;
        private set => _healthDepletionRate = value;
    }
#pragma warning restore

    private Vector3 lastFramePos;

    private void FixedUpdate()
    {
        if (isTimeBased || (lastFramePos - transform.position).magnitude > veloctyThreshold)
        {
            GameplayManager.Instance.Water -= WaterDepletionRate;
            GameplayManager.Instance.Faith -= FaithDepletionRate;

            if (shouldDepleteHealth && GameplayManager.Instance.Water <= waterHealthLosingThreshold)
            {
                foreach (var cultistInfo in GameplayManager.Instance.cultistInfos)
                {
                    cultistInfo.HP -= HealthDepletionRate;
                    if(cultistInfo.HP < 0.01f)
                    {
                        GameplayManager.Instance.cultistInfos.Remove(cultistInfo);
                    }
                }
            }
        }
        lastFramePos = transform.position;
    }
}
