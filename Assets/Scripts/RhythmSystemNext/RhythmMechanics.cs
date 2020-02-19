using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for handling of Combo, Rage and counting beat hit states for statistics
/// </summary>
public class RhythmMechanics : MonoBehaviour
{
    #region Variables

    // Singleton implementation
    public static RhythmMechanics Instance;

#pragma warning disable
    [Header("Statistics")]
    [SerializeField] private bool countBeats = true;
#pragma warning restore

    // Combo and Rage control
    public int Combo { get; private set; }
    public bool Rage { get; private set; }

    // Beat state counters
    public int BadBeats { get; private set; }
    public int GoodBeats { get; private set; }
    public int GreatBeats { get; private set; }
    public int PerfectBeats { get; private set; }

    // ----------------------------------------------------
    // ---- Mechanics events -------------------------------
    #region Events

    public event Action OnRageStart;
    public event Action OnRageStop;
    public event Action<int> OnComboChange;

    #endregion

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        if (Instance.IsRealNull())
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        OnRageStart += RageStartHandler;
        OnRageStop += RageStopHandler;
        OnComboChange += ComboChangeHandler;

        AudioTimeline.Instance.OnBarEnd += UpdateCombo;
        AudioTimeline.Instance.OnBeatFail += ResetCombo;

        if (countBeats)
            AudioTimeline.Instance.OnBeatHit += UpdateCounters;
    }

    private void OnDisable()
    {
        OnRageStart -= RageStartHandler;
        OnRageStop -= RageStopHandler;
        OnComboChange -= ComboChangeHandler;

        AudioTimeline.Instance.OnBarEnd -= UpdateCombo;
        AudioTimeline.Instance.OnBeatFail -= ResetCombo;

        if (countBeats)
            AudioTimeline.Instance.OnBeatHit -= UpdateCounters;
    }

    #endregion

    #region Component

    // ----------------------------------------------------
    // ---- Public methods for external use ---------------
    #region PublicMethods

    /// <summary>
    /// Count percent of given beat state in current context
    /// </summary>
    /// <param name="beatState">The state to count percent of.</param>
    /// <returns>Returns the percent of given beat state.</returns>
    public float CountPercent(BeatState beatState)
    {
        int beats = 0;
        switch (beatState)
        {
            case BeatState.None:
                return 0.0f;

            case BeatState.Bad:
                beats = BadBeats;
                break;
            case BeatState.Good:
                beats = GoodBeats;
                break;
            case BeatState.Great:
                beats = GreatBeats;
                break;
            case BeatState.Perfect:
                beats = PerfectBeats;
                break;
        }

        int sum = BadBeats + GoodBeats + GreatBeats + PerfectBeats;
        if (sum == 0)
            return -1.0f;
        else
            return (float)beats / sum;
    }

    #endregion

    // ----------------------------------------------------
    // ---- Private control methods -----------------------
    #region PrivateMethods

    private void UpdateCombo(BarState barState)
    {
        switch (barState)
        {
            case BarState.None:
            case BarState.Failed:
                return;

            case BarState.Average:
                Combo++;

                if (Combo > 9)
                    OnRageStart();
                else
                    OnRageStop();

                break;
            case BarState.Good:
                Combo++;

                if (Combo > 3)
                    OnRageStart();
                else
                    OnRageStop();

                break;
            case BarState.Perfect:
                Combo++;

                OnRageStart();

                break;
        }

        OnComboChange(Combo);
    }


    private void UpdateCounters(BeatState beatState, int beatNumber)
    {
        switch (beatState)
        {
            case BeatState.None:
                break;
            case BeatState.Bad:
                BadBeats++;
                break;
            case BeatState.Good:
                GoodBeats++;
                break;
            case BeatState.Great:
                GreatBeats++;
                break;
            case BeatState.Perfect:
                PerfectBeats++;
                break;
        }
    }

    private void RageStartHandler()
    {
        Rage = true;
    }

    private void RageStopHandler()
    {
        Rage = false;
    }

    private void IncrementCombo()
    {
        Combo++;
        OnComboChange(Combo);
    }

    private void ResetCombo()
    {
        Combo = 0;
        OnComboChange(Combo);
        OnRageStop();
    }

    private void ComboChangeHandler(int value)
    {

    }

    #endregion

    #endregion
}
