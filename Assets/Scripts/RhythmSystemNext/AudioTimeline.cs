using System;
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
    //      UNDER NO CIRCUMSTANCED REFER TO THIS VARIABLE DIRECTLY, IT SHOULD ONLY BE CHANGED
    //      BY CORRESPONDING METHODS
    
        private bool isPlaying = false;
    
    // -------------------------------------------------------------------------------------------
    
    private TimelineState timelineState = TimelineState.None;

    // Rhythm window tolerance values
    private double goodTolerance = 0.25f;
    private double greatTolerance = 0.15f;
    private double toleranceBias = 0.0f; // Use with caution as I don't have the patience to check if this value is correct

    // Audio timing control variables
    // Beat tracking
    private BeatState currentBeatState = BeatState.None;
    private double beatDuration;
    private double nextBeatMoment;
    private int currentBeatNumber = 0;
    private bool hasEncounteredPerfect = false;
    private bool wasCurrentBeatHit = false;

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

    // ----------------------------------------------------
    // ---- Timeline events -------------------------------
    
    public delegate void BeatEvent(bool isMain);
    public event BeatEvent OnBeat;
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
                        if (currentBeatNumber >= beatsPerBar)
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
                currentBeatState = BeatState.None;
                hasEncounteredPerfect = false;
                wasCurrentBeatHit = false;

                nextBeatMoment += beatDuration;
                currentBeatNumber++;
                if (currentBeatNumber >= beatsPerBar)
                {
                    currentBeatNumber = 0;
                }
            }
        }
    }
}
