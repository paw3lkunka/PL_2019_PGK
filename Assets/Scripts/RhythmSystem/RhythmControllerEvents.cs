using UnityEngine;
using System.Collections;

public partial class RhythmController : MonoBehaviour
{
    private void BeatHitBad()
    {
        OnComboEnd?.Invoke();
        combo = 0;
    }

    private void RageModeStart()
    {
        rageMode = true;
    }

    private void RageModeEnd()
    {
        rageMode = false;
    }

    private void EndBeat()
    {
        thisMeasureBeats[currentBeatNumber] = currentBeatStatus;
        currentBeatStatus = Beat.None;
        UpdateCounters();

        if (currentBeatNumber < timeSignature.x - 1)
        {
            currentBeatNumber++;
        }
        else
        {
            currentBeatNumber = 0;
            EvaluateCombo();
        }
    }

    private void EvaluateCombo()
    {
        int countBad = 0;
        int countGood = 0;
        int countGreat = 0;

        foreach (var beat in thisMeasureBeats)
        {
            switch (beat)
            {
                case Beat.None:
                    countBad++;
                    break;
                case Beat.Bad:
                    countBad++;
                    break;
                case Beat.Good:
                    countGood++;
                    break;
                case Beat.Great:
                    countGreat++;
                    break;
            }
        }

        if (countGood + countGreat == 4)
        {
            combo++;
            if ((countGreat == 4 && combo > 2) || combo > 8)
            {
                OnRageModeStart?.Invoke();
            }
            else
            {
                OnRageModeEnd?.Invoke();
            }
            if (combo == 1)
            {
                OnComboStart?.Invoke();
            }
        }
        else
        {
            combo = 0;
            OnComboEnd?.Invoke();
        }

        
        
    }

    private void UpdateCounters()
    {
        switch (currentBeatStatus)
        {
            case Beat.None:
                missedBeats++;
                break;
            case Beat.Bad:
                badBeats++;
                break;
            case Beat.Good:
                goodBeats++;
                break;
            case Beat.Great:
                greatBeats++;
                break;
        }
    }
}
