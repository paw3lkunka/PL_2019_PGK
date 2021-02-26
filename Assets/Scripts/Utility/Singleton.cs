using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T, K> : Singleton where T : MonoBehaviour where K : InstancingMode
{
    private static T _instance;
    private static readonly object Lock = new object();
    [SerializeField] protected bool _persistent = true;

    public static T Instance
    {
        get
        {
            // WHY IS THE STUPID QUITTING SETTING ITSELF TO TRUE???????
            //if (Quitting)
            //{
            //    Debug.LogWarning($"[{nameof(Singleton)}<{typeof(T)}>] Instance will not be returned because the application is quitting.");
            //    return null;
            //}
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

                if (typeof(K).Name == "ForbidLazyInstancing")
                {
                    // TODO: Better lazy instancing info because it throws this if we check for null
                    //Debug.LogError($"An insance of {typeof(T)} was requested, but {typeof(T)} doesn't allow lazy instancing!");
                    return null;
                }

                switch (typeof(T).Name)
                {
                    case "ApplicationManager":
                        Debug.Log($"<color=green>[ApplicationManager] An instance was created from prefab.</color>");
                        return _instance = Instantiate(((PrefabDatabase)Resources.Load("PrefabDatabase")).applicationManager).GetComponent<T>();
                    case "GameplayManager":
                        Debug.Log($"<color=green>[GameplayManager] An instance was created from prefab.</color>");
                        return _instance = Instantiate(((PrefabDatabase)Resources.Load("PrefabDatabase")).gameplayManager).GetComponent<T>();
                    case "WorldSceneManager":
                        Debug.Log($"<color=green>[WorldSceneManager] An instance was created from prefab.</color>");
                        return _instance = Instantiate(((PrefabDatabase)Resources.Load("PrefabDatabase")).worldSceneManager).GetComponent<T>();
                    default:
                        Debug.Log($"<color=blue>[{nameof(Singleton)}<{typeof(T)}>] An instance is needed in the scene and no existing instances were found, so a new instance will be created.</color>");
                        return _instance = new GameObject($"({nameof(Singleton)}){typeof(T)}").AddComponent<T>();
                }

            }
        }
    }

    protected virtual void Awake()
    {
        if (_persistent)
        {
            DontDestroyOnLoad(gameObject);
        }

        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}

public abstract class Singleton : MonoBehaviour
{
    [field:System.NonSerialized]
    public static bool Quitting { get; private set; } = false;

    private void OnApplicationQuit() 
    {
        Quitting = true;
    }
}

public class InstancingMode {}
public class AllowLazyInstancing : InstancingMode {}
public class ForbidLazyInstancing : InstancingMode {}