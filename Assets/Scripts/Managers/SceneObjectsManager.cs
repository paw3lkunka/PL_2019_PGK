using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectsManager : Singleton<SceneObjectsManager, ForbidLazyInstancing>
{
    public GameObject[] initAfterSceneLoadObjects;
    public GameObject[] disableBeforeSceneLoadObjects;
    //[Tooltip("Activates all objects in InitAfterSceneLoad array")]
#if UNITY_EDITOR
    private static bool debug = true;
#endif

    protected override void Awake()
    {
        base.Awake();
#if UNITY_EDITOR
        if(debug)
        {
            Debug.LogWarning("Initializing without scene loading");
            InitAfterSceneLoad();
            if(initAfterSceneLoadObjects.Length > 0)
            {
                initAfterSceneLoadObjects[0].GetComponentInChildren<AudioTimeline>()?.TimelineInit();
            }
            debug = false;
        }
#endif
    }

    public void InitAfterSceneLoad()
    {
        foreach(var obj in initAfterSceneLoadObjects)
        {
            if(!obj.activeInHierarchy)
            {
                obj.SetActive(true);
            }
        }
    }

    public void DisableBeforeSceneLoad()
    {
        foreach (var obj in disableBeforeSceneLoadObjects)
        {
            obj.SetActive(false);
        }
    }
}
