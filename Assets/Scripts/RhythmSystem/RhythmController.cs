using System;
using System.Runtime.Serialization;
using UnityEngine;
using TMPro;

public enum Beat { None, Bad, Good, Great };

public partial class RhythmController : MonoBehaviour
{
#pragma warning disable
    [Space]
    [Header("Song timing setup")]
    [SerializeField] private double songBpm;
    [Tooltip("Musical time signature, at the moment, only the beats per measure part is used")]
    [SerializeField] private Vector2Int timeSignature;

    [Space]
    [Header("Rhythm UI references")]
    [SerializeField] private bool debug = false;
    [SerializeField] TextMeshProUGUI comboText;
    [SerializeField] TextMeshProUGUI rageModeText;
#pragma warning restore

    // Singleton implementation
    public static RhythmController Instance { get; private set; }

    // Rhythm window tolerance values
    private double goodTolerance;
    private double greatTolerance;

    // Control variables
    private double beatTime;
    private double sequenceStartMoment;
    private double TimeSinceSequenceStart
    {
        get => AudioSettings.dspTime - sequenceStartMoment;
    }
    private double nextBeatMoment = 0.0f;
    private Beat currentBeatMomentStatus = Beat.None;

    // Combo variables
    private int combo = 0;
    public int Combo
    {
        get => combo;
    }
    private Beat[] thisMeasureBeats;
    private int currentBeatNumber = 0;
    private Beat currentBeatStatus = Beat.None;
    private bool rageMode = false;

    // Counters
    private int missedBeats = 0;
    private int badBeats = 0;
    private int goodBeats = 0;
    private int greatBeats = 0;

    // Indicator handling
    private double normalizedGoodTime = 0.0f;
    public double NormalizedGoodTime
    {
        get => normalizedGoodTime;
    }

    // Controller events
    public event Action OnBeatHitBad;
    public event Action OnBeatHitGood;
    public event Action OnBeatHitGreat;
    public event Action OnComboStart;
    public event Action OnComboEnd;
    public event Action OnRageModeStart;
    public event Action OnRageModeEnd;
    public event Action OnBeatEnd;

    public Beat HitBeat()
    {
        if (currentBeatStatus == Beat.None)
        {
            switch (currentBeatMomentStatus)
            {
                case Beat.Bad:
                    OnBeatHitBad?.Invoke();
                    break;
                case Beat.Good:
                    OnBeatHitGood?.Invoke();
                    break;
                case Beat.Great:
                    OnBeatHitGreat?.Invoke();
                    break;
            }
            currentBeatStatus = currentBeatMomentStatus;

            return currentBeatMomentStatus;
        }
        else
        {
            currentBeatStatus = Beat.Bad;
            OnBeatHitBad?.Invoke();
            return Beat.Bad;
        }
    }

    public void ResetSequence()
    {
        sequenceStartMoment = AudioSettings.dspTime;
        nextBeatMoment = TimeSinceSequenceStart + beatTime;
    }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("<color=red>You have two or more rhythm controllers in scene. Make sure there is always one rhythm controller!</color>");
        }

        goodTolerance = GameManager.Instance.goodTolerance;
        greatTolerance = GameManager.Instance.greatTolerance;

        OnBeatEnd += EndBeat;
        OnBeatHitBad += BeatHitBad;
        OnRageModeStart += RageModeStart;
        OnRageModeEnd += RageModeEnd;

        thisMeasureBeats = new Beat[timeSignature.x];
        beatTime = 60.0d / songBpm;
        if (greatTolerance > goodTolerance)
        {
            throw new BadBeatToleranceException("Great tolerance was set to higher than good tolerance, which is illegal. Fix this issue in the inspector");
        }

        OnRageModeStart += GameManager.Instance.ToRageMode;
        OnRageModeEnd += GameManager.Instance.ToNormalMode;
    }

    private void OnDestroy()
    {
        OnBeatEnd -= EndBeat;
        OnBeatHitBad -= BeatHitBad;
        OnRageModeStart -= RageModeStart;
        OnRageModeEnd -= RageModeEnd;
    }

    private void Update()
    {
        if (TimeSinceSequenceStart < nextBeatMoment - goodTolerance)
        {
            normalizedGoodTime = 0.0f;
            currentBeatMomentStatus = Beat.Bad;
        }
        else if (TimeSinceSequenceStart > nextBeatMoment - goodTolerance && TimeSinceSequenceStart < nextBeatMoment + goodTolerance)
        {
            if (TimeSinceSequenceStart < nextBeatMoment)
            {
                normalizedGoodTime = (TimeSinceSequenceStart - (nextBeatMoment - goodTolerance)) / (goodTolerance);
            }
            else
            {
                normalizedGoodTime = -((TimeSinceSequenceStart - nextBeatMoment) / (goodTolerance)) + 1.0f;
            }

            if (TimeSinceSequenceStart > nextBeatMoment - greatTolerance && TimeSinceSequenceStart < nextBeatMoment + greatTolerance)
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
            OnBeatEnd?.Invoke();
            nextBeatMoment += beatTime;
        }

        // Temporary input handling
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            HitBeat();
        }

        // Debug -----------
        if (debug)
        {
            comboText.text = "Combo: " + combo;
            if (rageMode)
            {
                rageModeText.text = "Rage mode!";
            }
            else
            {
                rageModeText.text = "";
            }
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