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
        hitIndicator.sharedMaterial.color = noneHitColor;
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

            rhythmIndicator.sharedMaterial.color = new Color(1.0f, 1.0f, 1.0f, rhythmCurve.Evaluate(indicatorFactor));

            var hitSize = hitIndicatorMaxSize - ((hitIndicatorMaxSize - 1) * indicatorFactor);
            hitIndicator.transform.localScale = new Vector3(hitSize, 1.0f, hitSize);

            var newHitColor = hitIndicator.sharedMaterial.color;
            newHitColor.a = hitCurve.Evaluate(1.0f - indicatorFactor);
            hitIndicator.sharedMaterial.color = newHitColor;
        }
    }

    #endregion

    #region Component

    private void VisualizeBeatHit(BeatState state, int beatNumber, bool primaryInteraction)
    {
        switch (state)
        {
            case BeatState.None:
                hitIndicator.sharedMaterial.color = noneHitColor;
                break;

            case BeatState.Bad:
                hitIndicator.sharedMaterial.color = badHitColor;
                break;

            case BeatState.Good:
                hitIndicator.sharedMaterial.color = goodHitColor;
                break;

            case BeatState.Great:
                hitIndicator.sharedMaterial.color = greatHitColor;
                break;

            case BeatState.Perfect:
                hitIndicator.sharedMaterial.color = perfectHitColor;
                break;

            default:
                hitIndicator.sharedMaterial.color = noneHitColor;
                break;
        }

        if (beatNumber <= 3 && state != BeatState.Bad)
        {
            beatHitSigns[beatNumber].material.color = hitIndicator.sharedMaterial.color;
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
