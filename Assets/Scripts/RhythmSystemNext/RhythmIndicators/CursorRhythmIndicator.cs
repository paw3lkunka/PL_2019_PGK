using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorRhythmIndicator : MonoBehaviour
{
    #region Variables

#pragma warning disable
    [Header("Rhythm pulse indicator")]
    [SerializeField] private SpriteRenderer rhythmIndicator;
    [SerializeField] private AnimationCurve rhythmCurve;
    [SerializeField] private float rhytmIndicatorMaxSize = 5.0f;

    [Space]
    
    [Header("Hit indicator")]
    [SerializeField] private SpriteRenderer hitIndicator;
    [SerializeField] private AnimationCurve hitCurve;
    [SerializeField] private float hitIndicatorMaxSize = 3.0f;
    [SerializeField] private List<SpriteRenderer> beatHitSigns = new List<SpriteRenderer>(3);
    
    [Space]

    [Header("Hit colors")]
    [SerializeField] private Color noneHitColor = Color.clear;
    [SerializeField] private Color badHitColor = Color.red;
    [SerializeField] private Color goodHitColor = Color.cyan;
    [SerializeField] private Color greatHitColor = Color.green;
    [SerializeField] private Color perfectHitColor = Color.magenta;
#pragma warning restore

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        hitIndicator.color = noneHitColor;
    }

    private void OnEnable()
    {
        if(AudioTimeline.Instance != default(AudioTimeline))
        {
            AudioTimeline.Instance.OnBeatHit += VisualizeBeatHit;
        }
    }

    private void OnDisable()
    {
        if (AudioTimeline.Instance != default(AudioTimeline))
        {
            AudioTimeline.Instance.OnBeatHit -= VisualizeBeatHit;
        }
    }

    private void Update()
    {
        if (AudioTimeline.Instance)
        {
            var indicatorFactor = AudioTimeline.Instance.NormalizedTimeUntilNextBeat;

            var rhythmSize = rhytmIndicatorMaxSize - ((rhytmIndicatorMaxSize - 1) * (1.0f - indicatorFactor));
            rhythmIndicator.transform.localScale = new Vector3(rhythmSize, rhythmSize, 0);

            rhythmIndicator.color = new Color(1.0f, 1.0f, 1.0f, rhythmCurve.Evaluate(indicatorFactor));

            var hitSize = hitIndicatorMaxSize - ((hitIndicatorMaxSize - 1) * indicatorFactor);
            hitIndicator.transform.localScale = new Vector3(hitSize, hitSize, 0);

            var newHitColor = hitIndicator.color;
            newHitColor.a = hitCurve.Evaluate(1.0f - indicatorFactor);
            hitIndicator.color = newHitColor;
        }
    }

    #endregion

    #region Component

    private void VisualizeBeatHit(BeatState state, int beatNumber)
    {
        switch (state)
        {
            case BeatState.None:
                hitIndicator.color = noneHitColor;
                break;

            case BeatState.Bad:
                hitIndicator.color = badHitColor;
                break;

            case BeatState.Good:
                hitIndicator.color = goodHitColor;
                break;

            case BeatState.Great:
                hitIndicator.color = greatHitColor;
                break;

            case BeatState.Perfect:
                hitIndicator.color = perfectHitColor;
                break;

            default:
                hitIndicator.color = noneHitColor;
                break;
        }

        if(beatNumber < 3 && state != BeatState.Bad)
        {
            beatHitSigns[beatNumber].color = hitIndicator.color;
        }
        else
        {
            ClearBeatHitSigns();
        }
    }

    private void ClearBeatHitSigns()
    {
        foreach (var bhs in beatHitSigns)
        {
            bhs.color = noneHitColor;
        }
    }

    #endregion
}
