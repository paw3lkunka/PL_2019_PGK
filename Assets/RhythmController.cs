using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using TMPro;

public enum Beat { None, Bad, Good, Great };

public class RhythmController : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private float songBpm;
    [Tooltip("Musical time signature, at the moment, only the beats per measure part is used")]
    [SerializeField] private Vector2Int timeSignature;

    [Space]
    [SerializeField] private float goodTolerance;
    [SerializeField] private float greatTolerance;

    [Space]
    [Header("Debug UI")]
    [SerializeField] private bool debug = false;
    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] TextMeshProUGUI currentBeatText;
    [SerializeField] TextMeshProUGUI rageModeText;
#pragma warning restore

    // External components
    private AudioSource audioSource;

    // Control variables
    private float beatTime = 0.0f;
    private float currentTimeMoment = 0.0f;
    private float nextBeatMoment = 0.0f;
    private Beat currentBeatMomentStatus = Beat.None;

    // Combo variables
    private int combo = 0;
    private Beat[] thisMeasureBeats;
    private int currentBeatNumber = 0;
    private Beat currentBeatStatus = Beat.None;
    private bool failedMeasure = false;
    private bool rageMode = false;

    // Counters
    private int missedBeats = 0;
    private int badBeats = 0;
    private int goodBeats = 0;
    private int greatBeats = 0;

    // Indicator handling
    private float normalizedGoodTime = 0.0f;

    public Beat HitBeat()
    {
        if (currentBeatStatus == Beat.None)
        {
            currentBeatStatus = currentBeatMomentStatus;
            return currentBeatMomentStatus;
        }
        else
        {
            FailCombo();
            currentBeatMomentStatus = Beat.Bad;
            return Beat.Bad;
        }
    }

    public float GetBeatAnimationTime()
    {
        return normalizedGoodTime;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        thisMeasureBeats = new Beat[timeSignature.x];
        beatTime = 60.0f / songBpm;
        if (greatTolerance > goodTolerance)
        {
            throw new BadBeatToleranceException("Great tolerance was set to higher than good tolerance, which is illegal. Fix this issue in the inspector");
        }
    }

    private void Update()
    {
        if (currentTimeMoment < nextBeatMoment - goodTolerance)
        {
            normalizedGoodTime = 0.0f;
            currentBeatMomentStatus = Beat.Bad;
        }
        else if (currentTimeMoment > nextBeatMoment - goodTolerance && currentTimeMoment < nextBeatMoment + goodTolerance)
        {
            if (currentTimeMoment < nextBeatMoment)
            {
                normalizedGoodTime = (currentTimeMoment - (nextBeatMoment - goodTolerance)) / (goodTolerance);
            }
            else
            {
                normalizedGoodTime = -((currentTimeMoment - nextBeatMoment) / (goodTolerance)) + 1.0f;
            }

            if (currentTimeMoment > nextBeatMoment - greatTolerance && currentTimeMoment < nextBeatMoment + greatTolerance)
            {
                currentBeatMomentStatus = Beat.Great;
            }
            else
            {
                currentBeatMomentStatus = Beat.Good;
            }
        }
        else
        {
            normalizedGoodTime = 0.0f;
            NextBeat();
            nextBeatMoment += beatTime;
        }


        // Debug -----------
        if (debug)
        {
            comboText.text = "Combo: " + combo;
            currentBeatText.text = "Beat: " + currentBeatNumber;
            if (rageMode)
            {
                rageModeText.text = "Rage mode!";
            }
            else
            {
                rageModeText.text = "";
            }
        }


        currentTimeMoment += Time.deltaTime;
    }

    private void NextBeat()
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
        if (failedMeasure)
        {
            failedMeasure = false;
            return;
        }

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

        combo++;

        if ( (countGreat == 4 && combo > 2) || combo > 8)
        {
            rageMode = true;
        }
        else
        {
            rageMode = false;
        }
    }

    private void FailCombo()
    {
        failedMeasure = true;
        combo = 0;
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

[Serializable]
internal class BadBeatToleranceException : Exception
{
    public BadBeatToleranceException()
    {
    }

    public BadBeatToleranceException(string message) : base(message)
    {
    }

    public BadBeatToleranceException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected BadBeatToleranceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}