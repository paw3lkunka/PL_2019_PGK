using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmAnimator : MonoBehaviour
{
#pragma warning disable
    [Header("Rhythm pulse indicator")]
    [SerializeField] private Image rhythmIndicator;
    [SerializeField] private AnimationCurve pulseCurve;
    [Space]
    [Header("Hit indicator")]
    [SerializeField] private Image hitIndicator;
    [SerializeField] private float animationTime;
    [SerializeField] private Color badHitColor = Color.red;
    [SerializeField] private Color goodHitColor = Color.yellow;
    [SerializeField] private Color greatHitColor = Color.green;
    [SerializeField] private AnimationCurve hitCurve;
    [Space]
    [Header("Camera effects")]
    [SerializeField] private bool cameraEffectsEnabled = true;
    [SerializeField] private AnimationCurve cameraCurve;
    [SerializeField] private float cameraEffectMultiplier = 0.05f;
    [SerializeField] private float rageMultiplier = 2.0f;
#pragma warning restore

    private Camera mainCamera;
    private float startCameraSize;
    private float coroutineTime;

    private bool cameraEffectsEnabledInternal = false;
    private float cameraEffectMultiplierInternal;

    private void Awake()
    {
        mainCamera = Camera.main;
        startCameraSize = mainCamera.orthographicSize;
        cameraEffectMultiplierInternal = cameraEffectMultiplier;
    }

    private void OnEnable()
    {
        RhythmController.Instance.OnBeatHitBad += BadHitAnimation;
        RhythmController.Instance.OnBeatHitGood += GoodHitAnimation;
        RhythmController.Instance.OnBeatHitGreat += GreatHitAnimation;
        RhythmController.Instance.OnComboStart += StartComboAnimation;
        RhythmController.Instance.OnComboEnd += EndComboAnimation;
        RhythmController.Instance.OnRageModeStart += StartRageAnimation;
        RhythmController.Instance.OnRageModeEnd += StopRageAnimation;
    }

    private void OnDisable()
    {
        RhythmController.Instance.OnBeatHitBad -= BadHitAnimation;
        RhythmController.Instance.OnBeatHitGood -= GoodHitAnimation;
        RhythmController.Instance.OnBeatHitGreat -= GreatHitAnimation;
        RhythmController.Instance.OnComboStart -= StartComboAnimation;
        RhythmController.Instance.OnComboEnd -= EndComboAnimation;
        RhythmController.Instance.OnRageModeStart -= StartRageAnimation;
        RhythmController.Instance.OnRageModeEnd -= StopRageAnimation;
    }

    private void BadHitAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(HitAnimation(badHitColor));
    }

    private void GoodHitAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(HitAnimation(goodHitColor));
    }

    private void GreatHitAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(HitAnimation(greatHitColor));
    }

    private void StartComboAnimation()
    {
        cameraEffectsEnabledInternal = true;
        cameraEffectMultiplierInternal = cameraEffectMultiplier;
    }

    private void EndComboAnimation()
    {
        cameraEffectsEnabledInternal = false;
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

    private void Update()
    {
        rhythmIndicator.color = new Color(  rhythmIndicator.color.r, 
                                            rhythmIndicator.color.g, 
                                            rhythmIndicator.color.b, 
                                            pulseCurve.Evaluate((float)RhythmController.Instance.NormalizedGoodTime));
        if (cameraEffectsEnabled && cameraEffectsEnabledInternal)
        {
            mainCamera.orthographicSize = startCameraSize - cameraCurve.Evaluate((float)RhythmController.Instance.NormalizedGoodTime) * cameraEffectMultiplierInternal;
        }
    }

    private IEnumerator HitAnimation(Color color)
    {
        coroutineTime = 0.0f;
        while (coroutineTime <= animationTime)
        {
            yield return new WaitForEndOfFrame();

            hitIndicator.color = new Color(color.r, color.g, color.b, hitCurve.Evaluate(coroutineTime / animationTime));

            coroutineTime += Time.deltaTime;
        }
        hitIndicator.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    }
}
