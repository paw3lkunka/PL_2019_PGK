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
    private bool isPlaying = false;
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

    public void BeatHit()
    {

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
            else
            {
                currentBeatState = BeatState.None;
            }

        }
    }
}
