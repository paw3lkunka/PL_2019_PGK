using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class HealthDepleter : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private float waterLevelTrigger = 0.0f; // TODO: Should probably be set in the manager
    [SerializeField] private float damageInterval = 1.0f;
    [SerializeField] private Vector2 damageRange;
    [SerializeField] private float damageChance = 0.3f;
#pragma warning restore

    private Damageable damageable;
    private bool isGettingDamaged = false;

    private void Awake()
    {
        damageable = GetComponent<Damageable>();
    }

    private void FixedUpdate()
    {
        if (!isGettingDamaged && GameplayManager.Instance.Water <= waterLevelTrigger)
        {
            isGettingDamaged = true;
            StartCoroutine(HealthDepletion());
        }
        else if (isGettingDamaged && GameplayManager.Instance.Water > waterLevelTrigger)
        {
            isGettingDamaged = false;
            StopAllCoroutines();
        }
    }

    private IEnumerator HealthDepletion()
    {
        while (isGettingDamaged)
        {
            if (Random.Range(0.0f, 1.0f) < damageChance)
            {
                damageable.DamageIgnoreDefense(Random.Range(damageRange.x, damageRange.y));
            }
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
