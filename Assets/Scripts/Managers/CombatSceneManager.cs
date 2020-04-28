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
    public List<Damageable> enemies;
    public List<Damageable> ourCrew;
    public _CultLeader cultLeader;
    [Tooltip("If checked, meneger automaticlly removes null emements from $ourCrew and $enemies")]
    public bool cleanLists = true;

    public Transform cultLeaderTransform;

    #region MonoBehaviour

    private void LateUpdate()
    {
        if(cleanLists)
        {
            enemies.RemoveAll((item) => !item || item == null);
            ourCrew.RemoveAll((item) => !item || item == null);
        }
    }

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
}
