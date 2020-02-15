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
        OnBeatFail -= BeatFailHandler;
    }

    private void BeatHandler(bool isMain)
    {

    }

    private void BeatHitHandler(BeatState beatState, int beatNumber)
    {

    }

    private void BeatFailHandler()
    {
        SequenceReset();
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
        nextBeatMoment = beatDuration;
    }

    private void SequenceResetHandler()
    {

    }

    private void SequencePauseHandler()
    {

    }

    private void SequenceResumeHandler()
    {

    }

    // Additional helper functions
    private IEnumerator SequenceResetCoroutine()
    {
        yield return new WaitForSeconds((float)beatDuration * failedBeatResetOffset);
        OnSequenceStart();
    }
}
