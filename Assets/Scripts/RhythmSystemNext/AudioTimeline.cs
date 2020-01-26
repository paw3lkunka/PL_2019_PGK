﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BeatState { None, Bad, Good, Great, Perfect };
public enum BarState { None, Average, Good, Perfect, Failed };
public enum TimelineState { None, Countup, PauseCountup, Playing, Paused, Interrupted };

public class AudioTimeline : MonoBehaviour
{
#pragma warning disable
    [Header("Timeline setup")]
    [SerializeField] private double songBpm = 80;
    [SerializeField] private int beatsPerBar = 4;
    [Header("Timing and delays")]
    [Tooltip("Time after failed beat for timeline to resume playing")]
    [SerializeField] private int failedBeatResetOffset = 4;
#pragma warning restore
    public int BeatsPerBar => beatsPerBar;
    public double SongBpm => songBpm;

    // Singleton implementation
    // Note that the timeline can be used to keep music synced between scenes if left "don't destroy on load"
    public static AudioTimeline Instance;

    // Timeline states

    // ---- Critical flag used for tracking if the timeline engine should be played or paused ----
    //      UNDER NO CIRCUMSTANCE REFER TO THIS VARIABLE DIRECTLY, IT SHOULD ONLY BE CHANGED
    //      BY CORRESPONDING METHODS
    
        private bool isPlaying = false;
    
    // -------------------------------------------------------------------------------------------
    
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
    private double nextBeatPauseMoment;
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
    public event BarEndEvent OnBarEnd;
    public event Action OnSequenceReset;
    public event Action OnPause;
    public event Action OnResume;

    // ----------------------------------------------------

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

    public void TimelineInit()
    {
        beatDuration = 60.0d / songBpm;
        SequenceStart();
    }

    public void TimelinePause()
    {
        // Set the timeline state to paused
        timelineState = TimelineState.Paused;

        // Save beat moment variables
        pauseMoment = AudioSettings.dspTime;
        nextBeatPauseMoment = nextBeatMoment;

        // Broadcast On Pause event
        OnPause();

        // Set timeline to not playing
        isPlaying = false;
    }

    public void TimelineResume()
    {
        // Set the timeline state to pause countup
        timelineState = TimelineState.PauseCountup;

        // Restore beat moment variables corrected by time passed
        nextBeatMoment = nextBeatPauseMoment + TimeSincePause;

        // Invoke On Resume event
        OnResume();

        // Set timeline to playing
        isPlaying = true;

        //TODO: Checking the number of beats for offset countup
    }

    private void SequenceStart()
    {
        // Reset sequence start moment and set next beat moment
        sequenceStartMoment = AudioSettings.dspTime;
        nextBeatMoment = beatDuration;
        // Set timeline state to countup
        timelineState = TimelineState.Countup;

        // Set the timeline state to is playing
        isPlaying = true;
    }

    public void SequenceReset()
    {
        // Set states accordingly
        isPlaying = false;
        timelineState = TimelineState.Interrupted;
        lastBarState = BarState.Failed;
        // Invoke sequence reset event
        OnSequenceReset();
        // Start sequence reset coroutine
        StartCoroutine(SequenceResetCoroutine());
    }

    private IEnumerator SequenceResetCoroutine()
    {
        yield return new WaitForSeconds((float)beatDuration * failedBeatResetOffset);
        SequenceStart();
    }

    /// <summary>
    /// The most important method, should be invoked by input mechanisms that handle rhythm
    /// Is responsible for determining at which state the beat was hit and invokes actions accordingly
    /// </summary>
    public void BeatHit()
    {
        Debug.Log("Beat hit invoked");
        if (wasCurrentBeatHit == false && 
            (timelineState == TimelineState.Countup || timelineState == TimelineState.Playing))
        {
            if (currentBeatState == BeatState.Bad || currentBeatState == BeatState.None)
            {
                SequenceReset();
            }

            // IF SOMETHING DOESN'T WORK YOU PROBABLY FORGOT TO RESET THIS FLAG
            wasCurrentBeatHit = true;
            // Finally record the beat state for bar evaluation
            barBeatStates[currentBeatNumber] = currentBeatState;
            // And send the event with current beat state attached to
            OnBeatHit(currentBeatState, currentBeatNumber);
        }

    }

    private void Update()
    {
        if (isPlaying)
        {
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
                        if (currentBeatNumber >= beatsPerBar - 1)
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
            else if (hasEncounteredPerfect == false)
            {
                currentBeatState = BeatState.None;
            }
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