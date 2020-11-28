using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorRhythmIndicator : MonoBehaviour
{
    #region Variables

#pragma warning disable
    [Header("Rhythm pulse indicator")]
    [SerializeField] private MeshRenderer rhythmIndicator;
    [SerializeField] private AnimationCurve rhythmCurve;
    [SerializeField] private float rhytmIndicatorMaxSize = 5.0f;

    [Space]
    
    [Header("Hit indicator")]
    [SerializeField] private MeshRenderer hitIndicator;
    [SerializeField] private AnimationCurve hitCurve;
    [SerializeField] private float hitIndicatorMaxSize = 3.0f;
    [SerializeField] private List<MeshRenderer> beatHitSigns = new List<MeshRenderer>(4);
    
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
        hitIndicator.material.color = noneHitColor;
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
            rhythmIndicator.transform.localScale = new Vector3(rhythmSize, 1.0f, rhythmSize);

            rhythmIndicator.material.color = new Color(1.0f, 1.0f, 1.0f, rhythmCurve.Evaluate(indicatorFactor));

            var hitSize = hitIndicatorMaxSize - ((hitIndicatorMaxSize - 1) * indicatorFactor);
            hitIndicator.transform.localScale = new Vector3(hitSize, 1.0f, hitSize);

            var newHitColor = hitIndicator.material.color;
            newHitColor.a = hitCurve.Evaluate(1.0f - indicatorFactor);
            hitIndicator.material.color = newHitColor;
        }
    }

    #endregion

    #region Component

    private void VisualizeBeatHit(BeatState state, int beatNumber, bool primaryInteraction)
    {
        switch (state)
        {
            case BeatState.None:
                hitIndicator.material.color = noneHitColor;
                break;

            case BeatState.Bad:
                hitIndicator.material.color = badHitColor;
                break;

            case BeatState.Good:
                hitIndicator.material.color = goodHitColor;
                break;

            case BeatState.Great:
                hitIndicator.material.color = greatHitColor;
                break;

            case BeatState.Perfect:
                hitIndicator.material.color = perfectHitColor;
                break;

            default:
                hitIndicator.material.color = noneHitColor;
                break;
        }

        if (beatNumber <= 3 && state != BeatState.Bad)
        {
            beatHitSigns[beatNumber].material.color = hitIndicator.material.color;
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
            bhs.material.color = noneHitColor;
        }
    }

    #endregion
}
