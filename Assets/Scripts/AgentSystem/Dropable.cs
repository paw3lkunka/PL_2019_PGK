using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Dropable : MonoBehaviour
{
    [SerializeField]
    private float dropChance = 0.0f;
    [SerializeField]
    private GameObject spawnedPrefab = null;

    /// <summary>
    /// How many [prefabs] will drop?
    /// </summary>
    [HideInInspector]
    public Vector2Int dropAmountRange;
    /// <summary>
    /// Position range of drop
    /// </summary>
    [HideInInspector]
    public Vector2 dropPositionRange;

    private void Start()
    {
        gameObject.GetComponent<Damageable>().Death += Drop;
    }

    private void Drop()
    {
        float random = UnityEngine.Random.Range(0.0f, 100.0f);
        Vector3 pos = transform.position;
        if (random < dropChance || random.Equals(dropChance))
        {
            for(int i = 0; i < UnityEngine.Random.Range(dropAmountRange.x, dropAmountRange.y); ++i)
            {
                Vector3 posMod = new Vector3(UnityEngine.Random.Range(dropPositionRange.x, dropPositionRange.y),
                                            0.0f,
                                            UnityEngine.Random.Range(dropPositionRange.x, dropPositionRange.y));
                if (spawnedPrefab.GetComponent<DropAnimation>() != null)
                {
                    GameObject obj = Instantiate(spawnedPrefab, pos, Quaternion.identity);
                    obj.SetActive(false);
                    obj.GetComponent<DropAnimation>().endPosition = posMod;
                    obj.SetActive(true);
                }
                else
                {
                    Instantiate(spawnedPrefab, pos + posMod, Quaternion.identity);
                }
            }
        }
    }
}
