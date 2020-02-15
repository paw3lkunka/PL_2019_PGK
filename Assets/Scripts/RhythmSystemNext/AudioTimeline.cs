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
    public double SongBpm => songBpm;
    [SerializeField] private int beatsPerBar = 4;
    public int BeatsPerBar => beatsPerBar;
    [Header("Timing and delays")]
    [Tooltip("Beats time after failed beat for timeline to resume playing")]
    [SerializeField] private int failedBeatResetOffset = 4;
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
    private double nextBeatMoment;
    public double NextBeatMoment => nextBeatMoment;
    private int currentBeatNumber = 0;
    private bool hasEncounteredPerfect = false;
    private bool wasCurrentBeatHit = false;
    private bool wasSequenceInitiated = false;

    // Pause saving moments
    private double pauseMoment;
    private double TimeSincePause
    {
        get => AudioSettings.dspTime - pauseMoment;
    }

    // Bar tracking
    private BeatState[] barBeatStates;
    private BarState lastBarState = BarState.None;

    // Sequence time variables
    private double sequenceStartMoment;
    private double TimeSinceSequenceStart
    {
        get => AudioSettings.dspTime - sequenceStartMoment;
    }
    public double TimeUntilNextBeat
    {
        get => nextBeatMoment - TimeSinceSequenceStart;
    }

    // ---- Combo handling --------------------------------

    public int Combo { get; private set; } = 0;

    // ----------------------------------------------------
    // ---- Timeline events -------------------------------

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

    // ----------------------------------------------------
    // ---- Public methods for external use ---------------

    public void Pause()
    {

    }

    public void Resume()
    {

    }

    /// <summary>
    /// The most important method, should be invoked by input mechanisms that handle rhythm
    /// Is responsible for determining at which state the beat was hit and invokes actions accordingly
    /// </summary>
    public void BeatHit(bool pause = false)
    {
        if (wasCurrentBeatHit == false &&
             (timelineState == TimelineState.Countup || timelineState == TimelineState.Playing))
        {
            if (currentBeatState == BeatState.Bad || currentBeatState == BeatState.None)
            {
                OnBeatFail();
            }

            // IF SOMETHING DOESN'T WORK YOU PROBABLY FORGOT TO RESET THIS FLAG
            wasCurrentBeatHit = true;
            // Finally record the beat state for bar evaluation
            barBeatStates[currentBeatNumber] = currentBeatState;
            // And send the event with current beat state attached to
            OnBeatHit(currentBeatState, currentBeatNumber);
        }
        else if (pause == true)
        {
            // Failed beat pause event

        }
    }

    // ----------------------------------------------------
    // ---- Unity functions -------------------------------
    // NOTE: OnEnable and OnDisable moved to AudioTimelineEvents

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

    private void Start()
    {
        TimelineInit();
    }

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
        if (TimeSinceSequenceStart > nextBeatMoment - goodTolerance + toleranceBias &&
            TimeSinceSequenceStart < nextBeatMoment + goodTolerance + toleranceBias)
        {

            currentBeatState = BeatState.Good;

            // The Great rhythm hit window condition is checked here
            if (TimeSinceSequenceStart > nextBeatMoment - greatTolerance &&
                TimeSinceSequenceStart < nextBeatMoment + greatTolerance)
            {

                currentBeatState = BeatState.Great;

                // ------------------------------------------------------------------------------
                // The Perfect rhythm hit condition is checked here
                if (TimeSinceSequenceStart >= nextBeatMoment && hasEncounteredPerfect == false)
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
            EvaluateBeat();

            // Resetting state keeping variables for the next beat
            currentBeatState = BeatState.None;
            hasEncounteredPerfect = false;
            wasCurrentBeatHit = false;

            // Advancing counters to next beat moment
            nextBeatMoment += beatDuration;

            // Increment beat number and reset it if the full bar has passed
            currentBeatNumber++;
            if (currentBeatNumber >= beatsPerBar)
            {
                EvaluateBar();
                currentBeatNumber = 0;
            }
        }

    }

    // ----------------------------------------------------
    // ---- Timeline control methods ----------------------

    private void TimelineInit()
    {
        beatDuration = 60.0d / songBpm;
        OnSequenceStart();
    }

    private void SequenceReset()
    {
        // ### TIMELINE STATE CHANGE ###
        timelineState = TimelineState.Interrupted;

        lastBarState = BarState.Failed;
        // Invoke sequence reset event
        OnSequenceReset();
        // Start sequence reset coroutine
        StartCoroutine(SequenceResetCoroutine());
    }

    private void SequencePause()
    {
        // ### TIMELINE STATE CHANGE ###
        timelineState = TimelineState.Paused;

        // Save beat moment variables
        pauseMoment = AudioSettings.dspTime;
        
        // Depending on 

        // Broadcast On Pause event
        OnPause();
    }

    private void SequenceResume()
    {
        // ### TIMELINE STATE CHANGE ###
        timelineState = TimelineState.Countup;

        // Restore beat moment variables corrected by time passed
        nextBeatMoment = nextBeatPauseMoment + TimeSincePause;

        // Invoke On Resume event
        OnResume();

        //TODO: Checking the number of beats for offset countup
    }

    private void EvaluateBeat()
    {
        // Take everything into account and update bar beat status
        if (!wasCurrentBeatHit)
        {
            barBeatStates[currentBeatNumber] = BeatState.None;
        }

        if (barBeatStates[currentBeatNumber] == BeatState.Bad)
        {
            FailSequence();
        }
    }

    private void FailSequence()
    {
        Combo = 0;
        SequenceReset();
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
            Combo += 2;
            OnBarEnd(BarState.Perfect);
        }
        else if (good == 0 && great > 0)
        {
            Combo++;
            OnBarEnd(BarState.Good);
        }
        else
        {
            Combo++;
            OnBarEnd(BarState.Average);
        }
    }
}


//switch (currentBeatState)
//{
//case BeatState.None:
//    // ---- On Beat State None ----
//    // If beat is hit here, it counts as a miss and should:
//    // - invoke appropriate method for handling fails
//    // - invoke appropriate event for failed beat broadcast
//    currentBeatState = BeatState.Bad;
//    Combo = 0;
//    SequenceReset();

//    break;
//case BeatState.Bad:
//    // ---- On Beat State Bad ----
//    // If beat is hit here, it is considered a duplicate hit in one beat
//    // Should be handled similarly to Beat State None
//    currentBeatState = BeatState.Bad; // Not necessary - potentially a lifesaver or a code breaker
//    Combo = 0;
//    SequenceReset();

//    break;
//case BeatState.Good:
//    // ---- On Beat State Good ----
//    // This condition handles beat state normally
//    currentBeatState = BeatState.Good;

//    break;
//case BeatState.Great:
//    // ---- On Beat State Great ----
//    // This condition handles beat state normally 
//    currentBeatState = BeatState.Great;

//    break;
//case BeatState.Perfect:
//    // ---- On Beat State Perfect ----
//    // This is a frame-perfect hit state and should be used only for special things
//    currentBeatState = BeatState.Perfect;

//    break;

//}