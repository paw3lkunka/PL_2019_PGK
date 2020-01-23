using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Location : MonoBehaviour
{
    [HideInInspector]
    public int spawnChance;
    [HideInInspector]
    public int generatorHashCode;

    private void OnValidate()
    {
        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
    }
}
