using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SustainedResource : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private bool isInfinite = false;
    [SerializeField] private float resourceAmount;
    [SerializeField] private float emissionAmount;
    [SerializeField] private float frequency;
#pragma warning restore

    public float StartResourceAmount { get; private set; }
    public float ResourceAmount { get => resourceAmount; }

    private TextEmitter textEmitter;

    private void Awake()
    {
        StartResourceAmount = resourceAmount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            textEmitter = other.GetComponentInChildren<TextEmitter>();
            if (textEmitter)
            {
                StartCoroutine(ResourceRoutine());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            textEmitter = null;
            StopAllCoroutines();
        }
    }

    private IEnumerator ResourceRoutine()
    {
        while (resourceAmount > 0.0f)
        {
            float emitted = Mathf.Min(emissionAmount, resourceAmount);
            if (!isInfinite)
            {
                resourceAmount -= emitted;
            }
            
            switch (resourceType)
            {
                case ResourceType.Water:
                    if (GameplayManager.Instance.Water.Max - GameplayManager.Instance.Water > 0.1f)
                    {
                        GameplayManager.Instance.Water += emitted;
                        textEmitter.Emit(ApplicationManager.Instance.PrefabDatabase.floatingTextResourceGained, emitted.ToString("0.0"), Color.cyan);
                    }
                    break;
                case ResourceType.Faith:
                    GameplayManager.Instance.Faith += emitted;
                    textEmitter.Emit(ApplicationManager.Instance.PrefabDatabase.floatingTextResourceGained, emitted.ToString("0.0"), Color.yellow);
                    break;
                case ResourceType.Health:
                    Debug.LogError("Health is undefined for sustained resource manager");
                    break;
            }
            yield return new WaitForSeconds(frequency);
        }
        resourceAmount = 0.0f;
    }
}
