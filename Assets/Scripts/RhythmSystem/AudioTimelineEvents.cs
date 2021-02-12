using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AudioTimeline
{

    #region Component

    // ----------------------------------------------------
    // ---- Internal event handlers -----------------------

    private void BeatHandler(bool isMain)
    {
        OnBeat?.Invoke(isMain);
    }

    private void BeatHitHandler(BeatState beatState, int beatNumber, bool primaryInteraction)
    {
        switch (beatState)
        {
            case BeatState.None:
            case BeatState.Bad:
                    BeatFailHandler();
                break;
            case BeatState.Good:
            case BeatState.Great:
            case BeatState.Perfect:
                break;
        }
        OnBeatHit?.Invoke(beatState, beatNumber, primaryInteraction);
    }

    private void BeatFailHandler()
    {
        hittingStarted = false;
        for (int i = 0; i < barBeatStates.Length; i++)
        {
            barBeatStates[i] = BeatState.None;
        }

        if (canFail)
        {
            SequenceResetHandler();
        }
        OnBeatFail?.Invoke(canFail);
    }

    private void BarEndHandler(BarState barState)
    {
        lastBarState = barState;
        OnBarEnd?.Invoke(barState);
    }

    private void BarSubdivHandler()
    {
        int noteMod = maxBarSubdivision;
        int noteSubdiv = 1;
        while (noteMod > 0)
        {
            if (currentSubdivNumber % noteMod == 0)
            {
                break;
            }
            noteMod /= 2;
            noteSubdiv *= 2;
        }
        //Debug.Log($"Note subdiv: {noteSubdiv}");
        OnSubdiv?.Invoke(noteSubdiv);
    }

    private void SequenceStartHandler()
    {
        // ### TIMELINE STATE CHANGE ###
        TimelineState = TimelineState.Countup;

        currentBeatNumber = 0;
        CountupCounter = 0;

        // Reset sequence start moment and set next beat moment
        sequenceStartMoment = AudioSettings.dspTime;
        NextBeatMoment = beatDuration;
        NextBarSubdivMoment = maxBarSubdivDuration;
        OnSequenceStart?.Invoke();
    }

    private void CountupEndHandler()
    {
        // ### TIMELINE STATE CHANGE ###
        TimelineState = TimelineState.Playing;
        currentBeatNumber = 0;
        CountupCounter = 0;
        OnCountupEnd?.Invoke();
    }

    private void SequenceResetHandler()
    {
        // ### TIMELINE STATE CHANGE ###
        TimelineState = TimelineState.Interrupted;

        lastBarState = BarState.Failed;

        OnSequenceReset?.Invoke();

        // Start sequence reset coroutine
        StartCoroutine(SequenceResetCoroutine());
    }

    private void SequencePauseHandler(bool keepCombo)
    {
        // ### TIMELINE STATE CHANGE ###
        TimelineState = TimelineState.Paused;
        this.keepCombo = keepCombo;

        // Save beat moment variables
        pauseMoment = AudioSettings.dspTime;
        OnSequencePause?.Invoke(keepCombo);
    }

    private void SequenceResumeHandler()
    {
        for (int i = 0; i < barBeatStates.Length; i++)
        {
            barBeatStates[i] = BeatState.None;
        }

        if (keepCombo)
        {
            SequenceStartHandler();
        }
        else
        {
            BeatFailHandler();
        }
        keepCombo = false;
        OnSequenceResume?.Invoke();
    }

    // Additional helper functions
    private IEnumerator SequenceResetCoroutine()
    {
        yield return new WaitForSeconds((float)beatDuration * failedBeatResetOffset);
        SequenceStartHandler();
    }

    #endregion
}