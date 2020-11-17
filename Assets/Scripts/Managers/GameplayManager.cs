using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ResourceType { Water, Faith, Health }

/// <summary>
/// Gameplay manager. Should be created at the start of game.
/// </summary>
// TODO: Add events
//      - OnCultistDie
//      - OnCultistAdd
public class GameplayManager : Singleton<GameplayManager, AllowLazyInstancing>
{
#pragma warning disable
    [Header("Basic resources")]
    [SerializeField] private Resource water = new Resource(100.0f, 100.0f);
    private float waterPercentLastFrame;
    public Resource Water 
    { 
        get => water;
        set => water.Set(value);
    }
    [SerializeField] private Resource faith = new Resource(25.0f, 35.0f);
    private float faithPercentLastFrame;
    public Resource Faith
    {
        get => faith;
        set => faith.Set(value);
    }
    [SerializeField] private Resource health = new Resource(0.0f, 0.0f);
    private float healthPercentLastFrame;
    public Resource Health
    {
        get => health;
        set => health.Set(value);
    }
#pragma warning restore

    [Header("Gameplay Config")] // * ===================================
    [ReadOnly]
    public int mapGenerationSeed; 
    public int initialCultistsNumber = 4;
    public float faithForKilledEnemy = 0.01f;
    public float faithForKilledCultist = 0.02f;
    public float faithForWoundedCultist = 0.001f;

    public float lowWaterLevel = 0.2f;
    public float lowFaithLevel = 0.2f;
    public float highFaithLevel = 0.7f;
    public float fanaticFaithLevel = 0.9f;

    public float faithBoost = 2.0f;

    // * ===== Pause handling =======================================

    public bool IsPaused { get; private set; }

    // * ===== Scene persistent crew ================================

    public List<CultistEntityInfo> cultistInfos;

    // * ===== Gameplay progress statistics =========================

    public uint shrinesToVisit = 3u;

    public HashSet<int> visitedObelisksIds = new HashSet<int>();
    public int markedShrineId = default;
    public bool obeliskActivated = false;

    public HashSet<int> visitedShrinesIds = new HashSet<int>();

    public bool leaderIsDead = false;
    public bool enteredTemple = false;
    private bool finalSceneLoaded = false;

    // * ===== Location variables ==========================================

    [HideInInspector]
    public bool firstTimeOnMap = true;
    /// <summary>
    /// Saved position from world map scene
    /// </summary>
    public Vector3 lastLocationPosition;
    public float lastLocationRadius;
    public int lastLocationId;

    //TODO reimplement
    public Dictionary<int, HashSet<int>> destroyedDynamicObjects = new Dictionary<int, HashSet<int>>();
    public HashSet<int> CurrentLocationsDestroyedDynamicObjects
    {
        get
        {
            try
            {
                return destroyedDynamicObjects[lastLocationId];
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
    }

    // * ===== Game events =================================================
    public event System.Action OnLocationEnter;
    public event System.Action OnLocationExit;

    // * ===== Resource value events ================================
    public event System.Action LowWaterLevelStart;
    public event System.Action LowWaterLevelEnd;

    public event System.Action LowFaithLevelStart;
    public event System.Action LowFaithLevelEnd;

    public event System.Action HighFaithLevelStart;
    public event System.Action HighFaithLevelEnd;

    public event System.Action FanaticStart;
    public event System.Action FanaticEnd;

    #region MonoBehaviour
    
    protected override void Awake()
    {
        base.Awake();
        // ? +++++ Init double buffered variables +++++
        waterPercentLastFrame = water.Normalized;
        faithPercentLastFrame = faith;

        for (int i = 0; i < initialCultistsNumber; i++)
        {
            cultistInfos.Add(new CultistEntityInfo(ApplicationManager.Instance.PrefabDatabase.cultists[0]));
        }

        Health.Max = 10.0f * initialCultistsNumber;
        Health.Set(10.0f * initialCultistsNumber);
        
        // ? +++++ Initialize shrine list +++++
        mapGenerationSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
    }

    private void Update() 
    {
        // ! ----- Game over condition -----
        if (!finalSceneLoaded && (leaderIsDead || enteredTemple))
        {
            finalSceneLoaded = true;
            ApplicationManager.Instance.GameOver(enteredTemple);
        }

        // ! ----- Game events update -----
        if (water.Normalized < lowWaterLevel && waterPercentLastFrame >= lowWaterLevel)
            LowWaterLevelStart?.Invoke();

        if (water.Normalized > lowWaterLevel && waterPercentLastFrame <= lowWaterLevel)
            LowWaterLevelEnd?.Invoke();

        if (faith.Normalized < lowFaithLevel && faithPercentLastFrame >= lowFaithLevel)
            LowFaithLevelStart?.Invoke();

        if (faith.Normalized > lowFaithLevel && faithPercentLastFrame <= lowFaithLevel)
            LowFaithLevelEnd?.Invoke();

        if (faith.Normalized > highFaithLevel && faithPercentLastFrame <= highFaithLevel)
            HighFaithLevelStart?.Invoke();

        if (faith.Normalized < highFaithLevel && faithPercentLastFrame >= highFaithLevel)
            HighFaithLevelEnd?.Invoke();

        if (faith.Normalized > fanaticFaithLevel && faithPercentLastFrame <= fanaticFaithLevel)
            FanaticStart?.Invoke();

        if (faith.Normalized < fanaticFaithLevel && faithPercentLastFrame >= fanaticFaithLevel)
            FanaticEnd?.Invoke();

        waterPercentLastFrame = water.Normalized;
        faithPercentLastFrame = faith.Normalized;
    }

    #endregion

    #region ManagerMethods

    /// <summary>
    /// Reset all resources and fields to their default values
    /// </summary>
    public void ResetResources()
    {
        water.Set(water.InitialValue);
        waterPercentLastFrame = 1.0f;
        faith.Set(faith.InitialValue);
        faithPercentLastFrame = 0.5f;
        visitedShrinesIds.Clear();
        visitedObelisksIds.Clear();
    }

    public void EnterLocation(Location location)
    {
        if (SceneManager.GetSceneByName(location.SceneName) != null)
        {
            OnLocationEnterInvoke();
            lastLocationPosition = location.transform.position;
            lastLocationRadius = location.radius * Mathf.Max(location.transform.localScale.x, location.transform.localScale.z);
            lastLocationId = location.id;
            SceneManager.LoadScene(location.SceneName);
        }
    }

    public void ExitLocation()
    {
        OnLocationExitInvoke();
        SceneManager.LoadScene(ApplicationManager.Instance.worldMapScene);
    }

    public void FaithBoostOn()
    {
        faithForKilledEnemy *= faithBoost;
        faithForKilledCultist *= faithBoost; // <-- Yes, that's intentional
    }

    public void FaithBoostOff()
    {
        faithForKilledEnemy /= faithBoost;
        faithForKilledCultist /= faithBoost;
    }

    public void OnLocationExitInvoke() => OnLocationExit?.Invoke();
    public void OnLocationEnterInvoke() => OnLocationEnter?.Invoke();

    public void MarkLastLocationAsVisitedShrine()
    {
        visitedShrinesIds.Add(lastLocationId);
        if(lastLocationId == markedShrineId)
        {
            markedShrineId = default;
        }
    }

    public void MarkLastLocationAsVisitedObelisk()
    {
        visitedObelisksIds.Add(lastLocationId);
        obeliskActivated = true;
    }

    public void PauseGame()
    {
        UIOverlayManager.Instance.PushToCanvas(ApplicationManager.Instance.PrefabDatabase.pauseGUI, PushBehaviour.Lock);
        IsPaused = true;
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        UIOverlayManager.Instance.PopFromCanvas();
        IsPaused = false;
        Time.timeScale = 1.0f;
    }

    public void TogglePause()
    {
        if (IsPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    #endregion
}
