using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
#pragma warning restore

    [Header("Gameplay Config")] // * ===================================
    [ReadOnly]
    public int mapGenerationSeed; 
    public int initialCultistsNumber = 4;
    public float faithForKilledEnemy = 0.01f;
    public float faithForKilledCultist = 0.02f;
    public float faithForWoundedCultist = 0.001f;
    public float faithUsageFactor = 0.0002f;
    public float waterUsageFactor = 0.0001f;

    public float lowWaterLevel = 0.2f;
    public float lowFaithLevel = 0.2f;
    public float highFaithLevel = 0.7f;
    public float fanaticFaithLevel = 0.9f;

    public float faithBoost = 2.0f;

    // * ===== Scene persistent crew ================================
    /// <summary>
    /// List of cultists (with leader at [0] )
    /// </summary>
    public List<GameObject> ourCrew;
    /// <summary>
    /// Saved position from world map scene
    /// </summary>
    public Vector3 lastWorldMapPosition;

    // * ===== Gameplay progress statistics =========================
    
    public List<Location> ShrinesVisited { get; set; }

    // * ===== Location variables ==========================================

    public Location currentLocation;
    public Dictionary<Location, HashSet<int>> destroyedDynamicObjects = new Dictionary<Location, HashSet<int>>();
    public HashSet<int> CurrentLocationsDestroyedDynamicObjects
    {
        get
        {
            try
            {
                return destroyedDynamicObjects[currentLocation];
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

        // ? +++++ Initialize shrine list +++++
        ShrinesVisited = new List<Location>();
        mapGenerationSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
    }

    private void Update() 
    {
        // ! ----- Game over condition -----
        if (ourCrew.Count <= 0)
        {
            ApplicationManager.Instance.GameOver(false);
        }

        // ! ----- Game events update -----
        if (water.Normalized < lowWaterLevel && waterPercentLastFrame >= lowWaterLevel)
            LowWaterLevelStart?.Invoke();

        if (water.Normalized > lowWaterLevel && waterPercentLastFrame <= lowWaterLevel)
            LowWaterLevelEnd?.Invoke();

        if (faith < lowFaithLevel && faithPercentLastFrame >= lowFaithLevel)
            LowFaithLevelStart?.Invoke();

        if (faith > lowFaithLevel && faithPercentLastFrame <= lowFaithLevel)
            LowFaithLevelEnd?.Invoke();

        if (faith > highFaithLevel && faithPercentLastFrame <= highFaithLevel)
            HighFaithLevelStart?.Invoke();

        if (faith < highFaithLevel && faithPercentLastFrame >= highFaithLevel)
            HighFaithLevelEnd?.Invoke();

        if (faith > fanaticFaithLevel && faithPercentLastFrame <= fanaticFaithLevel)
            FanaticStart?.Invoke();

        if (faith < fanaticFaithLevel && faithPercentLastFrame >= fanaticFaithLevel)
            FanaticEnd?.Invoke();

        waterPercentLastFrame = water;
        faithPercentLastFrame = faith;
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
        ShrinesVisited.Clear();
        RemoveCultistsFromCrew();
    }

    /// <summary>
    /// Destroy all cultist instances and clear list
    /// </summary>
    public void RemoveCultistsFromCrew()
    {
        for(int i = ourCrew.Count - 1; i >= 0; --i)
        {
            Destroy(ourCrew[i]);
        }
        ourCrew.Clear();
    }

    public void EnterLocation(string locationName)
    {
        if (SceneManager.GetSceneByName(locationName) != null)
        {
            OnLocationEnterInvoke();
            lastWorldMapPosition = GameObject.FindGameObjectWithTag("Leader").transform.position;
            SceneManager.LoadScene(locationName);
        }
    }

    public void ExitLocation()
    {
        OnLocationExitInvoke();
        SceneManager.LoadScene(ApplicationManager.Instance.worldMapScene);
        Cursor.visible = true;
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

    #endregion
}
