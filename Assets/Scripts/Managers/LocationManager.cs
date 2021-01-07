using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocationMode { Neutral, Hostile, Friendly }

/// <summary>
/// Keeps scene specific configuration
/// </summary>
public class LocationManager : Singleton<LocationManager, ForbidLazyInstancing>
{
    [Header("Base config")]
    public LocationMode sceneMode;
    public List<Damageable> enemies;
    public List<Damageable> ourCrew;
    public CultLeader cultLeader;
    public float formationScale = 1;
    public float stunCooldown;
    public float stunCounter;
    public bool CanStun { get => stunCounter <= 0; }

    [Tooltip("If checked, manager automatically removes null emements from $ourCrew and $enemies")]
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

    public static float offsetInLocation;

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
        QualitySettings.shadowDistance = 100.0f;
        GameplayManager.Instance.Faith.Overflowable = true;

        enemies.Clear();
        ourCrew.Clear();

        Vector3 leaderPosition = FindObjectOfType<CultLeader>().transform.position;
        int length = GameplayManager.Instance.cultistInfos.Count;

        for (int i = 0; i < length; i++)
        {
            Vector3 position = leaderPosition + formation[i] * formationScale;
            GameplayManager.Instance.cultistInfos[i].Instantiate(position, Quaternion.identity);
        }

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

        //if (sceneMode != LocationMode.Hostile)
        //{
        //    Cursor.visible = true;
        //}

        UIOverlayManager.Instance.ControlsSheet.Clear();
        UIOverlayManager.Instance.ControlsSheet.AddSheetElement(ButtonActionType.Walk, "Walk");

        if(sceneMode == LocationMode.Hostile)
        {
            UIOverlayManager.Instance.ControlsSheet.AddSheetElement(ButtonActionType.Shoot, "Shoot");
        }


        UIOverlayManager.Instance.ControlsSheet.AddSheetElement(ButtonActionType.Pause, "Pause");
    }

    private void Update()
    {
        stunCounter -= Time.deltaTime;
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

        AudioTimeline.Instance.OnBeatFail += OnBeatFail;
    }

    private void OnDisable()
    {
        if(AudioTimeline.Instance != null)
        {
            AudioTimeline.Instance.OnBeatFail -= OnBeatFail;
        }
    }

    private void OnBeatFail(bool reset)
    {
        if (CanStun && reset)
        {
            IEnumerator Routine()
            {
                yield return new WaitForEndOfFrame();
                stunCounter = stunCooldown;
            }

            StartCoroutine(Routine());
        }
    }

#endregion
}
