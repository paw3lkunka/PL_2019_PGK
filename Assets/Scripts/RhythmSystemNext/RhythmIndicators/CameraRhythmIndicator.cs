using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRhythmIndicator : MonoBehaviour
{
    #region Variables

#pragma warning disable
    [Header("Rhythm pulse indicator")]
    [SerializeField] private Image rhythmIndicator;
    [SerializeField] private AnimationCurve rhythmCurve;

    [Space]

    [Header("Hit indicator")]
    [SerializeField] private Image hitIndicator;
    [SerializeField] private AnimationCurve hitCurve;
    [SerializeField] [Range(0.0f, 1.0f)] private float hitIndicatorMinSize = 0.9f;

    [Space]

    [Header("Camera effects")]
    [SerializeField] private bool cameraEffectsEnabled = true;
    [SerializeField] private AnimationCurve cameraCurve;
    [SerializeField] private float cameraEffectMultiplier = 0.05f;
    [SerializeField] private float rageMultiplier = 2.0f;

    [Space]

    [Header("Hit colors")]
    [SerializeField] private Color noneHitColor = Color.clear;
    [SerializeField] private Color badHitColor = Color.red;
    [SerializeField] private Color goodHitColor = Color.cyan;
    [SerializeField] private Color greatHitColor = Color.green;
    [SerializeField] private Color perfectHitColor = Color.magenta;
#pragma warning restore

    private Camera mainCamera;
    private float startCameraSize;

    private bool cameraEffectsEnabledInternal = false;
    private float cameraEffectMultiplierInternal;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        mainCamera = Camera.main;
        startCameraSize = mainCamera.orthographicSize;
        cameraEffectMultiplierInternal = cameraEffectMultiplier;

        hitIndicator.color = noneHitColor;
    }

    private void OnEnable()
    {
        AudioTimeline.Instance.OnBeatHit += VisualizeBeatHit;

        RhythmMechanics.Instance.OnComboChange += HandleComboAnimation;

        RhythmMechanics.Instance.OnRageStart += StartRageAnimation;
        RhythmMechanics.Instance.OnRageStop += StopRageAnimation;
    }

    private void OnDisable()
    {
        AudioTimeline.Instance.OnBeatHit -= VisualizeBeatHit;

        RhythmMechanics.Instance.OnComboChange -= HandleComboAnimation;

        RhythmMechanics.Instance.OnRageStart -= StartRageAnimation;
        RhythmMechanics.Instance.OnRageStop -= StopRageAnimation;
    }

    private void Update()
    {
        if (AudioTimeline.Instance)
        {
            var indicatorFactor = AudioTimeline.Instance.NormalizedTimeUntilNextBeat;

            rhythmIndicator.color = new Color(1.0f, 1.0f, 1.0f, rhythmCurve.Evaluate(indicatorFactor));

            var hitSize = 1.0f - ((1.0f - hitIndicatorMinSize) * (1.0f - indicatorFactor));
            hitIndicator.transform.localScale = new Vector3(hitSize, hitSize, 0);

            var newHitColor = hitIndicator.color;
            newHitColor.a = hitCurve.Evaluate(1.0f - indicatorFactor);
            hitIndicator.color = newHitColor;

            if (cameraEffectsEnabled && cameraEffectsEnabledInternal)
            {
                mainCamera.orthographicSize = startCameraSize - cameraCurve.Evaluate(indicatorFactor) * cameraEffectMultiplierInternal;
            }
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
    }

    private void HandleComboAnimation(int combo)
    {
        switch(combo)
        {
            case 0:
                cameraEffectsEnabledInternal = false;
                break;

            case 1:
                cameraEffectsEnabledInternal = true;
                cameraEffectMultiplierInternal = cameraEffectMultiplier;
                break;
        }
    }

    private void StartRageAnimation()
    {
        cameraEffectsEnabledInternal = true;
        cameraEffectMultiplierInternal = cameraEffectMultiplier * rageMultiplier;
    }

    private void StopRageAnimation()
    {
        cameraEffectMultiplierInternal = cameraEffectMultiplier;
    }

    #endregion
}
