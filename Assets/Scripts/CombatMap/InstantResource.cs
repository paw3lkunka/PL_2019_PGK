using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextEmitter))]
public class InstantResource : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private ResourceType type;
    [SerializeField] private float amount;
#pragma warning restore

    private TextEmitter textEmitter;

    private void Awake()
    {
        textEmitter = GetComponent<TextEmitter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (type)
        {
            case ResourceType.Water:
                GameplayManager.Instance.Water += amount;
                textEmitter.Emit(ApplicationManager.Instance.PrefabDatabase.floatingTextResourceGained, amount.ToString("0.0"), Color.cyan);
                break;
            case ResourceType.Faith:
                GameplayManager.Instance.Faith += amount;
                textEmitter.Emit(ApplicationManager.Instance.PrefabDatabase.floatingTextResourceGained, amount.ToString("0.0"), Color.yellow);
                break;
            case ResourceType.Health:
                var damageable = other.GetComponent<Damageable>();
                if (damageable)
                {
                    damageable.Heal(amount);
                    textEmitter.Emit(ApplicationManager.Instance.PrefabDatabase.floatingTextResourceGained, amount.ToString("0.0"), Color.green);
                }
                break;
        }
        Destroy(gameObject);
    }
}
