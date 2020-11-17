using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class HealthDepleter : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private float waterLevelTrigger = 0.0f; // TODO: Should probably be set in the manager
    [SerializeField] private bool isTimeBased = false;
    [SerializeField] private float veloctyThreshold = 0.01f;
    [SerializeField] private float damageChance = 0.3f;

    [SerializeField] private float _healthDepletionRate;
    public float HealthDepletionRate
    {
        get => _healthDepletionRate;
        private set => _healthDepletionRate = value;
    }
#pragma warning restore

    private Vector3 lastFramePos;
    private Damageable damageable;

    private void Awake()
    {
        damageable = GetComponent<Damageable>();
    }

    private void FixedUpdate()
    {
        if (GameplayManager.Instance.Water <= waterLevelTrigger)
        {
            if (isTimeBased || (lastFramePos - transform.position).magnitude > veloctyThreshold)
            {
                if (Random.Range(0.0f, 1.0f) < damageChance)
                {
                    damageable.DamageIgnoreDefense(HealthDepletionRate);
                }
            }
        }
        lastFramePos = transform.position;
    }

}
