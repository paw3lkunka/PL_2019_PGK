using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BeatState { None, Bad, Good, Great, Perfect };
public enum BarState { None, Average, Good, Perfect, Failed };
public enum TimelineState { None, Countup, Playing, Interrupted };

public class AudioTimeline : MonoBehaviour
{
#pragma warning disable
    [Header("Timeline setup")]
    [SerializeField] private double songBpm = 80;
    [SerializeField] private int beatsPerBar = 4;
#pragma warning restore
    public int BeatsPerBar => beatsPerBar;
    public double SongBpm => songBpm;

    // Singleton implementation
    // Note that the timeline can be used to keep music synced between scenes if left "don't destroy on load"
    public static AudioTimeline Instance;

    // Timeline states

    // ---- Critical flag used for tracking if the timeline engine should be played or paused ----
    //      UNDER NO CIRCUMSTANCED REFER TO THIS VARIABLE DIRECTLY, IT SHOULD ONLY BE CHANGED
    //      BY CORRESPONDING METHODS
    
        private bool isPlaying = false;
    
    // -------------------------------------------------------------------------------------------
    
    private TimelineState timelineState = TimelineState.None;

    // Rhythm window tolerance values
    private double goodTolerance;
    private double greatTolerance;
    private double toleranceBias; // Use with caution as I don't have the patience to check if this value is correct

    // Audio timing control variables
    // Beat tracking
    private BeatState currentBeatState = BeatState.None;
    private double beatDuration;
    private double nextBeatMoment;
    private int currentBeatNumber = 0;
    private bool hasEncounteredPerfect = false;
    private bool wasCurrentBeatHit = false;

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

    // Timeline events
    public delegate void BeatEvent(bool isMain);
    public event BeatEvent OnBeat;

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
        SequenceStart();
    }

    public void TimelineInit()
    {
        beatDuration = 60.0d / songBpm;
    }

    public void TimelinePause()
    {

    }

    public void TimelineResume()
    {

    }

    private void SequenceStart()
    {
        isPlaying = true;

        sequenceStartMoment = AudioSettings.dspTime;
        nextBeatMoment = beatDuration;
    }

    /// <summary>
    /// The most important method, should be invoked by input mechanisms that handle rhythm
    /// Is responsible for determining at which state the beat was hit and invokes actions accordingly
    /// </summary>
    public void BeatHit()
    {
        if (wasCurrentBeatHit == false)
        {
            // IF SOMETHING DOESN'T WORK YOU PROBABLY FORGOT TO RESET THIS FLAG
            wasCurrentBeatHit = true;

            switch (currentBeatState)
            {
                case BeatState.None:
                    // ---- On Beat State None ----
                    // If beat is hit here, it counts as a miss and should:
                    // - invoke appropriate method for handling fails
                    // - invoke appropriate event for failed beat broadcast
                    currentBeatState = BeatState.Bad;

                    break;
                case BeatState.Bad:
                    // ---- On Beat State Bad ----
                    // If beat is hit here, it is considered a duplicate hit in one beat
                    // Should be handled similarly to Beat State None
                

                    break;
                case BeatState.Good:
                    // ---- On Beat State Good ----
                    // This condition handles beat state normally
                

                    break;
                case BeatState.Great:
                    // ---- On Beat State Great ----
                    // This condition handles beat state normally 


                    break;
                case BeatState.Perfect:
                    // ---- On Beat State Perfect ----
                    // This is a frame-perfect hit state and should be used only for special things


                    break;

            }
            // Finally record the beat state for bar evaluation
            barBeatStates[currentBeatNumber] = currentBeatState;
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
                        if (currentBeatNumber == beatsPerBar)
                        {
                            currentBeatNumber = 0;
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
                currentBeatState = BeatState.None;
                hasEncounteredPerfect = false;
                wasCurrentBeatHit = false;
            }
        }
    }
}
