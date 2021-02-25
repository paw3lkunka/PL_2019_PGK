using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocationMode { Neutral, Hostile, Friendly }

/// <summary>
/// Keeps scene specific configuration
/// </summary>
public class LocationManager : Singleton<LocationManager, ForbidLazyInstancing>
{
    public static Vector3 enterDirection;
    public static Vector3 exitDirection;

    [Header("Base config")]
    public float locationRadius;
    public LocationMode sceneMode;
    public List<Damageable> enemies;
    public List<Damageable> ourCrew;
    public CultLeader cultLeader;
    public float formationScale = 1;
    public float stunCooldown;
    public float stunCounter;
    public bool CanStun { get => stunCounter <= 0; }

    [Tooltip("If checked, manager automatically removes null elements from $ourCrew and $enemies")]
    public bool cleanLists = true;
    [Range(0.0f, 1.0f), Tooltip("How many % of enemies must be killed to end the location")]
    public float locationEndKillPercantage = 0.8f;

    [HideInInspector] public bool isExiting = false;

    private Vector3 toCenter;
    private int startEnemies;

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

        ExitZone[] exitZones = FindObjectsOfType<ExitZone>();

        EnterZone[] enterZones = FindObjectsOfType<EnterZone>();
        float[] angles = new float[enterZones.Length];
        Vector3 direction;
        for (int i = 0; i < enterZones.Length; i++)
        {
            direction = transform.position - enterZones[i].transform.position;
            direction.y = 0.0f;
            direction = direction.normalized;
            angles[i] = Vector3.Angle(enterDirection, direction);
        }

        float smallestAngle = angles[0];
        int bestIndex = 0;
        for (int i = 0; i < angles.Length; i++)
        {
            if (angles[i] < smallestAngle)
            {
                smallestAngle = angles[i];
                bestIndex = i;
            }
        }

        toCenter = transform.position - enterZones[bestIndex].transform.position;
        toCenter.Normalize();
        float yRotationToCenter = Vector3.SignedAngle(Vector3.forward, toCenter.normalized, Vector3.up);
        Quaternion rotation = Quaternion.AngleAxis(yRotationToCenter, Vector3.up);
        cultLeader = Instantiate(ApplicationManager.Instance.PrefabDatabase.cultLeader, enterZones[bestIndex].transform.position, rotation).GetComponent<CultLeader>();

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
        startEnemies = enemies.Count;

        CalculateOffsets();

        RepaintUI();
    }

    private void Update()
    {
        stunCounter -= Time.deltaTime;

        if (Vector3.Distance(cultLeader.transform.position, transform.position) > locationRadius || isExiting)
        {
            EnterExitLocationManager.Instance.IsEnteringExiting = true;
        }
        else
        {
            EnterExitLocationManager.Instance.IsEnteringExiting = false;
        }

        if (EnterExitLocationManager.Instance.EnterExitProgressNormalized >= 0.99f)
        {
            Exit();
        }
    }

    private void LateUpdate()
    {
        if(cleanLists)
        {
            enemies.RemoveAll((item) => !item || item == null);
            ourCrew.RemoveAll((item) => !item || item == null);
        }

        if((enemies.Count / (float)startEnemies) < (1.0f - locationEndKillPercantage))
        {
            for(int i = 0; i < enemies.Count; ++i)
            {
                var pos = enemies[i].gameObject.transform.position;
                var rot = enemies[i].gameObject.transform.rotation;
                var parent = enemies[i].transform.parent;
                enemies[i].gameObject.SetActive(false);
                Instantiate(ApplicationManager.Instance.PrefabDatabase.recruit, pos, rot, parent);
                Destroy(enemies[i].gameObject);
            }
            enemies.Clear();
            sceneMode = LocationMode.Neutral;
            RepaintUI();
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, locationRadius);
        if (cultLeader)
            Gizmos.DrawLine(cultLeader.transform.position, cultLeader.transform.position + toCenter);
    }
    #endregion

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

    private void Exit()
    {
        Vector3 vec = cultLeader.transform.position - transform.position;
        vec.y = 0;
        exitDirection = vec.normalized;
        GameplayManager.Instance.ExitLocation();
    }


    private void RepaintUI()
    {
        UIOverlayManager.Instance.ControlsSheet.Clear();
        UIOverlayManager.Instance.ControlsSheet.AddSheetElement(ButtonActionType.Walk, "Walk");

        if(sceneMode == LocationMode.Hostile)
        {
            UIOverlayManager.Instance.ControlsSheet.AddSheetElement(ButtonActionType.Shoot, "Shoot");
        }

        UIOverlayManager.Instance.ControlsSheet.AddSheetElement(ButtonActionType.Pause, "Pause");
    }


}
