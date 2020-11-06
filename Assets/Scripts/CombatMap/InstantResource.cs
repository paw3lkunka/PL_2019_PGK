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

    public ResourceType Type { get => type; }

    //for barrels - but cam be used in other way
    public bool dissappearOnCollect = true;
    [HideInInspector] public Material onEmptyMaterial;
    [HideInInspector] public Material onFullMaterial;
    [HideInInspector] public GameObject indicator;
    private bool full = true;

    // for wineskins - water pickups
    [HideInInspector] public float appendMaxValue;

    private TextEmitter textEmitter;

    private void Awake()
    {
        textEmitter = GetComponent<TextEmitter>();
        if (!dissappearOnCollect)
        {
            indicator.GetComponent<MeshRenderer>().material = onFullMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerCrew"))
        {
            if (dissappearOnCollect)
            {
                AddResource(other);
                Destroy(gameObject);
            }
            else if (full)
            {
                indicator.GetComponent<MeshRenderer>().material = onEmptyMaterial;
                AddResource(other);
                full = false;
            }
            else
            {
                // DO NOTHING!
            }
        }
    }

    private void AddResource(Collider other)
    {
        switch (type)
        {
            case ResourceType.Water:
                GameplayManager.Instance.Water += amount;
                textEmitter.Emit(ApplicationManager.Instance.PrefabDatabase.floatingTextResourceGained, amount.ToString("0.0"), Color.cyan);
                if(dissappearOnCollect && appendMaxValue > 0.0001f)
                {
                    GameplayManager.Instance.Water.Max += appendMaxValue;
                }
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
    }
}
