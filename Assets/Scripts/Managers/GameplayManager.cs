using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gameplay manager. Should be created at the start of game.
/// </summary>
// TODO: Add events
//      - OnCultistDie
//      - OnCultistAdd
public class GameplayManager : Singleton<GameplayManager>
{
#pragma warning disable
    [Header("Basic resources")]
    // TODO: Implement resource class with max, min and value
    [Range(0, 1), SerializeField] private float water = 1.0f;
    private float waterLastFrame;
    public float Water 
    { 
        get => water; 
        set => water = Mathf.Clamp01(value);
    }
    [Range(0, 1), SerializeField] private float faith = 0.5f;
    private float faithLastFrame;
    public float Faith
    {
        get => faith;
        set => faith = Mathf.Clamp01(value);
    }
#pragma warning restore

    [Header("Gameplay Config")] // * ===================================
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
    
    public bool mapGenerated;
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

    private void Awake() 
    {
        // ? +++++ Init double buffered variables +++++
        waterLastFrame = water;
        faithLastFrame = faith;

        // ? +++++ Initialize shrine list +++++
        ShrinesVisited = new List<Location>();
    }

    private void Update() 
    {
        // ! ----- Game over condition -----
        if (ourCrew.Count <= 0)
        {
            ApplicationManager.Instance.GameOver(false);
        }

        // ! ----- Game events update -----
        if (water < lowWaterLevel && waterLastFrame >= lowWaterLevel)
            LowWaterLevelStart?.Invoke();

        if (water > lowWaterLevel && waterLastFrame <= lowWaterLevel)
            LowWaterLevelEnd?.Invoke();

        if (faith < lowFaithLevel && faithLastFrame >= lowFaithLevel)
            LowFaithLevelStart?.Invoke();

        if (faith > lowFaithLevel && faithLastFrame <= lowFaithLevel)
            LowFaithLevelEnd?.Invoke();

        if (faith > highFaithLevel && faithLastFrame <= highFaithLevel)
            HighFaithLevelStart?.Invoke();

        if (faith < highFaithLevel && faithLastFrame >= highFaithLevel)
            HighFaithLevelEnd?.Invoke();

        if (faith > fanaticFaithLevel && faithLastFrame <= fanaticFaithLevel)
            FanaticStart?.Invoke();

        if (faith < fanaticFaithLevel && faithLastFrame >= fanaticFaithLevel)
            FanaticEnd?.Invoke();

        waterLastFrame = water;
        faithLastFrame = faith;
    }

    #endregion

    #region ManagerMethods

    /// <summary>
    /// Reset all resources and fields to their default values
    /// </summary>
    public void ResetResources()
    {
        water = 1.0f;
        waterLastFrame = 1.0f;
        faith = 0.5f;
        faithLastFrame = 0.5f;
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
