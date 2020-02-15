using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AudioTimeline
{
    private void OnEnable()
    {
        OnBeat += BeatHandler;
        OnBeatHit += BeatHitHandler;
        OnBeatFail += BeatFailHandler;
        OnBarEnd += BarEndHandler;
        OnSequenceStart += SequenceStartHandler;
        OnSequenceReset += SequenceResetHandler;
        OnSequencePause += SequencePauseHandler;
        OnSequenceResume += SequenceResumeHandler;
    }

    private void OnDisable()
    {
        OnBeat -= BeatHandler;
        OnBeatHit -= BeatHitHandler;
        OnBeatFail -= BeatFailHandler;
        OnBarEnd -= BarEndHandler;
        OnSequenceStart -= SequenceStartHandler;
        OnSequenceReset -= SequenceResetHandler;
        OnSequencePause -= SequencePauseHandler;
        OnSequenceResume -= SequenceResumeHandler;
    }

    // ----------------------------------------------------
    // ---- Internal event handlers -----------------------

    private void BeatHandler(bool isMain)
    {

    }

    private void BeatHitHandler(BeatState beatState, int beatNumber)
    {

    }

    private void BeatFailHandler()
    {
        OnSequenceReset();
    }

    private void BarEndHandler(BarState barState)
    {

    }

    private void SequenceStartHandler()
    {
        // ### TIMELINE STATE CHANGE ###
        timelineState = TimelineState.Countup;

        // Reset sequence start moment and set next beat moment
        sequenceStartMoment = AudioSettings.dspTime;
        NextBeatMoment = beatDuration;
    }

    private void SequenceResetHandler()
    {
        // ### TIMELINE STATE CHANGE ###
        timelineState = TimelineState.Interrupted;

        lastBarState = BarState.Failed;
        // Invoke sequence reset event
        OnSequenceReset();
        // Start sequence reset coroutine
        StartCoroutine(SequenceResetCoroutine());
    }

    private void SequencePauseHandler()
    {
        // ### TIMELINE STATE CHANGE ###
        timelineState = TimelineState.Paused;

        // Save beat moment variables
        pauseMoment = AudioSettings.dspTime;
    }

    private void SequenceResumeHandler()
    {
        // ### TIMELINE STATE CHANGE ###
        timelineState = TimelineState.Countup;

        // Restore beat moment variables corrected by time passed
        NextBeatMoment = AudioSettings.dspTime + beatDuration;
    }

    // Additional helper functions
    private IEnumerator SequenceResetCoroutine()
    {
        yield return new WaitForSeconds((float)beatDuration * failedBeatResetOffset);
        OnSequenceStart();
    }
}