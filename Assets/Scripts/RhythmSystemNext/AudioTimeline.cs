using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BeatState { None, Bad, Good, Great, Perfect };
public enum BarState { None, Average, Good, Perfect, Failed };
public enum TimelineState { None, Countup, Playing, Paused, Interrupted };

public partial class AudioTimeline : MonoBehaviour
{
#pragma warning disable
    [Header("Timeline setup")]
    [SerializeField] private double songBpm = 80;
    [SerializeField] private int beatsPerBar = 4;
    [Header("Timing and delays")]
    [Tooltip("Beats time after failed beat for timeline to resume playing")]
    [SerializeField] private int failedBeatResetOffset = 4;
    public double SongBpm => songBpm;
    public int BeatsPerBar => beatsPerBar;
#pragma warning restore

    // Singleton implementation
    // Note that the timeline can be used to keep music synced between scenes if left "don't destroy on load"
    public static AudioTimeline Instance;

    // Timeline states
    
    private TimelineState timelineState = TimelineState.None;

    // Rhythm window tolerance values
    private double goodTolerance = 0.25f;
    private double greatTolerance = 0.08f;
    private double toleranceBias = 0.0f; // Use with caution as I don't have the patience to check if this value is correct

    // Audio timing control variables
    // Beat tracking
    private BeatState currentBeatState = BeatState.None;
    private double beatDuration;
    public double NextBeatMoment { get; private set; }
    private int currentBeatNumber = 0;
    private bool hasEncounteredPerfect = false;
    private bool wasCurrentBeatHit = false;

    // Pause saving moments
    private double pauseMoment;
    private double TimeSincePause
    {
        get => AudioSettings.dspTime - pauseMoment;
    }

    // Bar tracking
    private BeatState[] barBeatStates;

    // Sequence time variables
    private double sequenceStartMoment;
    private double TimeSinceSequenceStart
    {
        get => AudioSettings.dspTime - sequenceStartMoment;
    }
    public double TimeUntilNextBeat
    {
        get => NextBeatMoment - TimeSinceSequenceStart;
    }

    // ----------------------------------------------------
    // ---- Timeline events -------------------------------
    #region Events

    public delegate void BeatEvent(bool isMain);
    public delegate void BeatHitEvent(BeatState beatState, int beatNumber);
    public delegate void BarEndEvent(BarState barState);
    public event BeatEvent OnBeat;
    public event BeatHitEvent OnBeatHit;
    public event Action OnBeatFail;
    public event BarEndEvent OnBarEnd;
    public event Action OnSequenceStart;
    public event Action OnSequenceReset;
    public event Action OnSequencePause;
    public event Action OnSequenceResume;

    #endregion

    // ----------------------------------------------------
    // ---- Public methods for external use ---------------
    #region PublicMethods

    /// <summary>
    /// Should be used similarly to BeatHit and works like this:
    /// - if invoked not during a beat pauses immediately and invokes OnBeatFail on resume
    /// - if invoked during a valid beat moment waits for another four pause inputs and pauses without failing
    /// </summary>
    public void Pause()
    {
        if (timelineState == TimelineState.Playing)
        {
            if (wasCurrentBeatHit == false)
            {
                wasCurrentBeatHit = true;
                
                if (currentBeatState == BeatState.Bad || currentBeatState == BeatState.None)
                {
                    OnSequencePause();
                }
                else
                {
                    // And send the event with current beat state attached to
                    OnBeatHit(currentBeatState, currentBeatNumber);
                }
                // Record the beat state for bar evaluation
                barBeatStates[currentBeatNumber] = currentBeatState;
            }
            else
            {
                OnSequencePause();
            }
        }
    }

    /// <summary>
    /// Resumes paused timeline, does nothing if timeline isn't paused
    /// </summary>
    public void Resume()
    {
        if (timelineState == TimelineState.Paused)
        {
            OnSequenceResume();
        }
    }

    /// <summary>
    /// The most important method, should be invoked by input mechanisms that handle rhythm
    /// Is responsible for determining at which state the beat was hit and invokes actions accordingly
    /// </summary>
    public void BeatHit()
    {
        if (timelineState == TimelineState.Countup || timelineState == TimelineState.Playing)
        {
            if (wasCurrentBeatHit == false)
            {

                if (currentBeatState == BeatState.Bad || currentBeatState == BeatState.None)
                {
                    OnBeatFail();
                }
                else
                {
                    OnBeatHit(currentBeatState, currentBeatNumber);
                }
                wasCurrentBeatHit = true;
                // Always record the beat state for bar evaluation
                barBeatStates[currentBeatNumber] = currentBeatState;
            }
            else
            {
                OnBeatFail();
            }
        }
    }

    #endregion

    // ---------------------------------------------------------
    // ---- Unity methods --------------------------------------
    //
    // NOTE: OnEnable and OnDisable moved to AudioTimelineEvents
    #region UnityMethods

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

        barBeatStates = new BeatState[beatsPerBar];
    }

    private void Start() => TimelineInit();

    private void Update()
    {
        // Make a check if update should check for beat and fire events
        // If yes, verify if events should fire as countup or not
        switch (timelineState)
        {
            case TimelineState.None:
            case TimelineState.Paused:
            case TimelineState.Interrupted:
                return; // The rhythm is not playing so return and do nothing
                        // TODO: Important part, should verify its validity

            case TimelineState.Countup:
                // Set additional variables for countup
                break;
            case TimelineState.Playing:
                // Set additional variables for normal
                break;
        }


        // ---- Main rhythm check ----
        // The Good rhythm hit window condition is checked here
        if (TimeSinceSequenceStart > NextBeatMoment - goodTolerance + toleranceBias &&
            TimeSinceSequenceStart < NextBeatMoment + goodTolerance + toleranceBias)
        {

            currentBeatState = BeatState.Good;

            // The Great rhythm hit window condition is checked here
            if (TimeSinceSequenceStart > NextBeatMoment - greatTolerance &&
                TimeSinceSequenceStart < NextBeatMoment + greatTolerance)
            {

                currentBeatState = BeatState.Great;

                // ------------------------------------------------------------------------------
                // The Perfect rhythm hit condition is checked here
                if (TimeSinceSequenceStart >= NextBeatMoment && hasEncounteredPerfect == false)
                {

                    hasEncounteredPerfect = true;
                    currentBeatState = BeatState.Perfect;

                    // ---- Invoke the OnBeat event with corresponding parameter ----
                    if (currentBeatNumber == beatsPerBar - 1)
                    {
                        OnBeat(true);
                    }
                    else
                    {
                        OnBeat(false);
                    }

                }
                // ------------------------------------------------------------------------------
            }
        }

        // If beat is not detected (i.e. the time is not in timeframe)
        // set the beat state to none because the time is before beat
        else if (hasEncounteredPerfect == false)
        {
            currentBeatState = BeatState.None;
        }

        // This fires after last possible condition of good beat
        // so this calls the beat evaluation method and sets next beat states
        else if (hasEncounteredPerfect == true)
        {
            // The reset state when the beat evaluation should take place
            if (!wasCurrentBeatHit)
            {
                barBeatStates[currentBeatNumber] = BeatState.None;
            }

            // Resetting state keeping variables for the next beat
            currentBeatState = BeatState.None;
            hasEncounteredPerfect = false;
            wasCurrentBeatHit = false;

            // Advancing counters to next beat moment
            NextBeatMoment += beatDuration;

            // Increment beat number and reset it if the full bar has passed
            currentBeatNumber++;
            if (currentBeatNumber >= beatsPerBar)
            {
                EvaluateBar();
                currentBeatNumber = 0;
            }
        }

    }

    #endregion

    // ----------------------------------------------------
    // ---- Timeline control methods ----------------------
    #region TimelineControl

    private void TimelineInit()
    {
        beatDuration = 60.0d / songBpm;
        OnSequenceStart();
    }

    private void EvaluateBar()
    {
        int good = 0, great = 0, perfect = 0, bad = 0, none = 0;

        // Didn't take into account the fail states because they are managed earlier
        // This may be a BUG place if something goes wrong
        foreach (var state in barBeatStates)
        {
            switch (state)
            {
                case BeatState.None:
                    none++;
                    break;
                case BeatState.Bad:
                    bad++;
                    break;
                case BeatState.Good:
                    good++;
                    break;
                case BeatState.Great:
                    great++;
                    break;
                case BeatState.Perfect:
                    perfect++;
                    break;
            }
        }

        if (bad > 0)
        {
            OnBarEnd(BarState.Failed);
        }
        else if (none > 0)
        {
            if (none == beatsPerBar)
            {
                OnBarEnd(BarState.None);
            }
            else
            {
                OnBarEnd(BarState.Failed);
            }
        }
        else if (perfect == beatsPerBar)
        {
            OnBarEnd(BarState.Perfect);
        }
        else if (good == 0 && great > 0)
        {
            OnBarEnd(BarState.Good);
        }
        else
        {
            OnBarEnd(BarState.Average);
        }
    }

    #endregion

}