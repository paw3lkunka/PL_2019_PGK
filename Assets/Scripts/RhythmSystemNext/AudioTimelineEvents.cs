using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AudioTimeline
{
    #region Variables



    #endregion

    #region MonoBehaviour



    #endregion

    #region Component

    // ----------------------------------------------------
    // ---- Internal event handlers -----------------------

    private void BeatHandler(bool isMain)
    {
        OnBeat?.Invoke(isMain);
    }

    private void BeatHitHandler(BeatState beatState, int beatNumber)
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
        OnBeatHit?.Invoke(beatState, beatNumber);
    }

    private void BeatFailHandler()
    {
        hittingStarted = false;
        for (int i = 0; i < barBeatStates.Length; i++)
        {
            barBeatStates[i] = BeatState.None;
        }
        SequenceResetHandler();
        OnBeatFail?.Invoke();
    }

    private void BarEndHandler(BarState barState)
    {
        lastBarState = barState;
        OnBarEnd?.Invoke(barState);
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

        // TODO: Probably not the best idea to do it this way so loot at it later
        Instantiate(GameManager.Instance.pauseScreen);
        Time.timeScale = 0;

        // Save beat moment variables
        pauseMoment = AudioSettings.dspTime;
        OnSequencePause?.Invoke(keepCombo);
    }

    private void SequenceResumeHandler()
    {
        pauseCounter = 0;
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