﻿using System.Collections;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : MonoBehaviour
{
    [field: SerializeField]
    public int ID { get; private set; }

#if UNITY_EDITOR
    public void SetId(int value) => ID = value;
#endif

    private void OnEnable()
    {
        if (GameManager.Instance.CurrentLocationsDestroyedDynamicObjects?.Contains(ID) ?? false)
        {
            Destroy(gameObject);
        }
    }

    public void RememberAsDestroyed()
    {
        HashSet<int> value = null;
        try
        {
            value = GameManager.Instance.destroyedDynamicObjects[GameManager.Instance.currentLocation];
        }
        catch(KeyNotFoundException)
        {
            GameManager.Instance.destroyedDynamicObjects.Add(GameManager.Instance.currentLocation, new HashSet<int>());
            value = GameManager.Instance.destroyedDynamicObjects[GameManager.Instance.currentLocation];
        }
        finally
        {
            value.Add(ID);
        }
    }
    public void DestroyAndRemember()
    {
        GameManager.Instance.destroyedDynamicObjects[GameManager.Instance.currentLocation].Add(ID);
        Destroy(gameObject);
    }

}