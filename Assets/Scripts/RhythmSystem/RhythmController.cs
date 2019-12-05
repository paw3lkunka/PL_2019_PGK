using System;
using System.Runtime.Serialization;
using UnityEngine;
using TMPro;

public enum Beat { None, Bad, Good, Great };

public partial class RhythmController : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private AudioSource lightGuitar;
    [SerializeField] private AudioSource heavyGuitar;

    [SerializeField] private int startOffset = 0;
    [SerializeField] private float fineTune = 0.0f;
    [SerializeField] private float songBpm;
    [Tooltip("Musical time signature, at the moment, only the beats per measure part is used")]
    [SerializeField] private Vector2Int timeSignature;

    [Space]
    [Header("Debug UI")]
    [SerializeField] private bool debug = false;
    [SerializeField] TextMeshProUGUI comboText;
    //[SerializeField] TextMeshProUGUI currentBeatText;
    [SerializeField] TextMeshProUGUI rageModeText;
#pragma warning restore

    // Singleton implementation
    public static RhythmController Instance { get; private set; }

    private float goodTolerance;
    private float greatTolerance;

    // External components
    private AudioSource audioSource;

    // Control variables
    private float beatTime;
    private float timeSinceEnable;
    private float TimeSinceEnable
    {
        get => Time.time - timeSinceEnable;
    }
    private float nextBeatMoment = 0.0f;
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
    private float normalizedGoodTime = 0.0f;
    public float NormalizedGoodTime
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
            currentBeatStatus = currentBeatMomentStatus;
            if (currentBeatMomentStatus == Beat.Good)
            {
                OnBeatHitGood?.Invoke();
            }
            else if (currentBeatMomentStatus == Beat.Great)
            {
                OnBeatHitGreat?.Invoke();
            }
            else if (currentBeatMomentStatus == Beat.Bad)
            {
                OnBeatHitBad?.Invoke();
            }
            return currentBeatMomentStatus;
        }
        else
        {
            currentBeatStatus = Beat.Bad;
            OnBeatHitBad?.Invoke();
            return Beat.Bad;
        }
    }

    private void Awake()
    {
        Instance = this;

        goodTolerance = GameManager.Instance.goodTolerance;
        greatTolerance = GameManager.Instance.greatTolerance;

        //else
        //{
        //    Debug.Log("<color=red>You have two or more rhythm controllers in scene. Make sure there is always one rhythm controller!</color>");
        //}

        OnBeatEnd += EndBeat;
        OnBeatHitBad += BeatHitBad;
        OnRageModeStart += RageModeStart;
        OnRageModeEnd += RageModeEnd;

        thisMeasureBeats = new Beat[timeSignature.x];
        beatTime = 60.0f / songBpm;
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

    private void OnEnable()
    {
        timeSinceEnable = Time.time;

        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        lightGuitar.Play();
        heavyGuitar.Play();

        nextBeatMoment = TimeSinceEnable + (startOffset * beatTime) + fineTune;
    }

    private void Update()
    {
        if (TimeSinceEnable < nextBeatMoment - goodTolerance)
        {
            normalizedGoodTime = 0.0f;
            currentBeatMomentStatus = Beat.Bad;
        }
        else if (TimeSinceEnable > nextBeatMoment - goodTolerance && TimeSinceEnable < nextBeatMoment + goodTolerance)
        {
            if (TimeSinceEnable < nextBeatMoment)
            {
                normalizedGoodTime = (TimeSinceEnable - (nextBeatMoment - goodTolerance)) / (goodTolerance);
            }
            else
            {
                normalizedGoodTime = -((TimeSinceEnable - nextBeatMoment) / (goodTolerance)) + 1.0f;
            }

            if (TimeSinceEnable > nextBeatMoment - greatTolerance && TimeSinceEnable < nextBeatMoment + greatTolerance)
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
            //currentBeatText.text = "Beat: " + currentBeatNumber;
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