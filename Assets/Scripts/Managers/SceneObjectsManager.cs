using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectsManager : Singleton<SceneObjectsManager, ForbidLazyInstancing>
{
    public GameObject[] initAfterSceneLoadObjects;
    public GameObject[] disableBeforeSceneLoadObjects;
    [Tooltip("Activates all objects in InitAfterSceneLoad array")]
    public bool debug = false;

    void Start()
    {
        if(debug)
        {
            InitAfterSceneLoad();
            initAfterSceneLoadObjects[0].GetComponentInChildren<AudioTimeline>().TimelineInit();
        }
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
