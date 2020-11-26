using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField] private Resource water = new Resource(100.0f, 100.0f, false);
    private float waterPercentLastFrame;
    public Resource Water 
    { 
        get => water;
        set => water.Set(value);
    }
    [Header("Read only - controlled by start amount of cultists and faith percent")]
    [SerializeField] private Resource faith = new Resource(25.0f, 35.0f, true);
    private float faithPercentLastFrame;
    public Resource Faith
    {
        get => faith;
        set => faith.Set(value);
    }
    [SerializeField] private float startFaithPercent = 0.75f;

    public float Health
    {
        get
        {
            float allHealth = ApplicationManager.Instance.PrefabDatabase.cultLeader.GetComponent<Damageable>().Health;
            foreach (var cultist in cultistInfos)
            {
                allHealth += cultist.HP;
            }
            return allHealth;
        }
    }

    public float MaxHealth
    {
        get
        {
            float allHealth = ApplicationManager.Instance.PrefabDatabase.cultLeader.GetComponent<Damageable>().Health.Max;
            foreach (var cultist in cultistInfos)
            {
                allHealth += cultist.HP.Max;
            }
            return allHealth;
        }
    }

    public bool IsAvoidingFight
    {
        get
        {
            return avoidingFightTimer > maxAvoidingFightTime;
        }
    }

    [HideInInspector]public float avoidingFightTimer = 0.0f;

#pragma warning restore

    [Header("Gameplay Config")] // * ===================================
    [ReadOnly]
    public int mapGenerationSeed; 
    public int initialCultistsNumber = 4;
    public float faithPerCultist = 20.0f;
    public float cultistWoundedFaith = 0.1f;

    public float lowWaterLevel = 0.2f;
    public float lowFaithLevel = 0.2f;
    public float highFaithLevel = 0.7f;
    public float fanaticFaithLevel = 0.9f;

    public float faithBoost = 2.0f;

    public float maxAvoidingFightTime = 240.0f;
    public float avoidingFightsFaithDebuf = 0.6f;

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

    private void OnEnable()
    {
        ApplicationManager.Instance.Input.Gameplay.Pause.performed += TogglePause;
        ApplicationManager.Instance.Input.Gameplay.Pause.Enable();
    }

    private void OnDisable()
    {
        ApplicationManager.Instance.Input.Gameplay.Pause.performed -= TogglePause;
        ApplicationManager.Instance.Input.Gameplay.Pause.Disable();
    }

    protected override void Awake()
    {
        base.Awake();

        // ? +++++ Faith init +++++
        faith.Max = new Resource(cultistInfos.Count * faithPerCultist * startFaithPercent, cultistInfos.Count * faithPerCultist, true);

        for (int i = 0; i < initialCultistsNumber; i++)
        {
            cultistInfos.Add(new CultistEntityInfo(ApplicationManager.Instance.PrefabDatabase.cultists[0]));
        }

        // ? +++++ Init double buffered variables +++++
        waterPercentLastFrame = water.Normalized;
        faithPercentLastFrame = faith.Normalized;

        // ? +++++ Initialize shrine list +++++
        mapGenerationSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
    }

    private void Update() 
    {
        // ! ----- Max faith amount -----
        faith.Max = cultistInfos.Count * faithPerCultist;

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

        if(!IsPaused)
        {
            avoidingFightTimer += Time.deltaTime;
        }
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
        AudioTimeline.Instance?.Pause();
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        UIOverlayManager.Instance.PopFromCanvas();
        IsPaused = false;
        AudioTimeline.Instance?.Resume();
        Time.timeScale = 1.0f;
    }

    public void TogglePause(InputAction.CallbackContext ctx)
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

    public void DecreseFaithByCultistWounded() => faith -= cultistWoundedFaith;

    #endregion
}
