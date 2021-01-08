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

    void Start()
    {
#if UNITY_EDITOR
        if(debug)
        {
            InitAfterSceneLoad();
            initAfterSceneLoadObjects[0].GetComponentInChildren<AudioTimeline>().TimelineInit();
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
