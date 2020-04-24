using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatSceneMode { Neutral, Hostile, Friendly }

/// <summary>
/// Keeps scene specific configuration
/// </summary>
public class CombatSceneManager : Singleton<CombatSceneManager>
{
    [Header("Base config")]
    public CombatSceneMode sceneMode;
    public Transform startPoint;
    public List<GameObject> enemies; // TODO: Change GameObject to enemy component

    public Transform CultLeaderTransform { get; private set; }

#region MonoBehaviour

    private void OnEnable() 
    {
        UIOverlayManager.Instance.PushToCanvas(ApplicationManager.Instance.prefabDatabase.combatSceneGUI);
        UIOverlayManager.Instance.PushToCanvas(ApplicationManager.Instance.prefabDatabase.rhythmGUI);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startPoint.position, .2f);
    }

#endregion

#region Editor

    #if UNITY_EDITOR
    private int nextId = 1;
    private void OnValidate()
    {
        foreach ( var obj in FindObjectsOfType<DynamicObject>())
        {
            if (obj.ID == 0)
            {
                obj.SetId(nextId++);
            }
        }
    }
    #endif

#endregion
}
