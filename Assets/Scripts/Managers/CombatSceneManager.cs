using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatSceneMode { Neutral, Hostile, Friendly }

/// <summary>
/// Keeps scene specific configuration
/// </summary>
public class CombatSceneManager : Singleton<CombatSceneManager, ForbidLazyInstancing>
{
    [Header("Base config")]
    public CombatSceneMode sceneMode;
    public Transform startPoint;
    public List<Damageable> enemies;
    public List<Damageable> ourCrew;
    public CultLeader cultLeader;
    public float formationScale = 1;
    [Tooltip("If checked, meneger automaticlly removes null emements from $ourCrew and $enemies")]
    public bool cleanLists = true;


    public void CalculateOffsets()
    {
        int i = 1;
        foreach (Damageable d in ourCrew)
        {
            var behav = d.GetComponent<BehaviourUserInput>();
            if(behav)
            {
                behav.formationOffset = formation[i++] * formationScale;
            }
        }
    }

    /// <summary>
    /// FORMATION LAYOUT: (0 - cult leader) <br/>
    ///
    ///     8 6 4 6 8       <br/>
    ///     7 3 1 2 7       <br/>
    ///     5 3 0 2 5       <br/>
    ///     7 3 1 2 7       <br/>
    ///     8 6 4 6 8       <br/>
    ///     
    /// </summary>
    public static readonly Vector3[] formation = 
    {
        //1
        new Vector3(0, 0, -1), new Vector3(0, 0, 1),
        //2
        new Vector3(1, 0, 0), new Vector3(1, 0, -1), new Vector3(1, 0, 1),
        //3
        new Vector3(-1, 0, 0), new Vector3(-1, 0, -1), new Vector3(-1, 0, 1),
        //4
        new Vector3(0, 0, -2), new Vector3(0, 0, 2),
        //5
        new Vector3(2, 0, 0), new Vector3(-2, 0, 0),
        //6
        new Vector3(1, 0, -2), new Vector3(1, 0, 2), new Vector3(-1, 0, -2), new Vector3(-1, 0, 2),
        //7
        new Vector3(2, 0, -1), new Vector3(2, 0, 1), new Vector3(-2, 0, -1), new Vector3(-2, 0, 1),
        //9
        new Vector3(2, 0, -2), new Vector3(2, 0, 2), new Vector3(-2, 0, -2), new Vector3(-2, 0, 2),
    };

    #region MonoBehaviour

    private void Start()
    {
        enemies.Clear();
        ourCrew.Clear();

        foreach(var d in FindObjectsOfType<Damageable>())
        {
            if (d.gameObject.GetComponent<Cultist>())
            {
                ourCrew.Add(d);
            }
            else if (d.gameObject.layer == LayerMask.NameToLayer("Enemies"))
            {
                enemies.Add(d);
            }
        }

        CalculateOffsets();
    }

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
        UIOverlayManager.Instance.PushToCanvas(ApplicationManager.Instance.PrefabDatabase.combatSceneGUI);
        UIOverlayManager.Instance.PushToCanvas(ApplicationManager.Instance.PrefabDatabase.rhythmGUI);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (startPoint)
        {
            Gizmos.DrawSphere(startPoint.position, .2f);
        }
    }

#endregion
}
