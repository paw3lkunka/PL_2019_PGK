using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : Singleton where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object Lock = new object();
    [SerializeField] private bool _persistent = true;

    public static T Instance
    {
        get
        {
            if (Quitting)
            {
                Debug.LogWarning($"[{nameof(Singleton)}<{typeof(T)}>] Instance will not be returned because the application is quitting.");
                return null;
            }
            lock (Lock)
            {
                if (_instance != null)
                {
                    return _instance;
                }
                var instances = FindObjectsOfType<T>();
                var count = instances.Length;
                if (count > 0)
                {
                    if (count == 1)
                    {
                        return _instance = instances[0];
                    }

                    Debug.LogWarning($"[{nameof(Singleton)}<{typeof(T)}>] There should never be more than one {nameof(Singleton)} of type {typeof(T)} in the scene, but {count} were found. The first instance found will be used, and all others will be destroyed.");
                    
                    for (var i = 1; i < instances.Length; ++i)
                    {
                        Destroy(instances[i]);
                    }

                    return _instance = instances[0];
                }

                Debug.Log($"<color=blue>[{nameof(Singleton)}<{typeof(T)}>] An instance is needed in the scene and no existiong instances were found, so a new instance will be created.</color>");
                return _instance = new GameObject($"({nameof(Singleton)}){typeof(T)}").AddComponent<T>();
            }
        }
    }

    private void Awake()
    {
        if (_persistent)
        {
            DontDestroyOnLoad(gameObject);
        }
        OnAwake();
    }

    protected virtual void OnAwake() {}
}

public abstract class Singleton : MonoBehaviour
{
    public static bool Quitting { get; private set; }

    private void OnApplicationQuit() 
    {
        Quitting = true;    
    }
}