using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveMusicController : MonoBehaviour
{
#pragma warning disable
    [Header("Base audio and clips")]
    // External components
    [SerializeField] private AudioSource drumTrack;
    [SerializeField] private AudioSource lightTrack;
    [SerializeField] private AudioSource heavyTrack;

    // Count up track
    [SerializeField] private bool countUp = true;
    [SerializeField] private AudioClip countUpClip;
    // Audio loop clips arrays
    [SerializeField] private AudioClip[] drumTrackClips;
    [SerializeField] private AudioClip[] lightTrackClips;
    [SerializeField] private AudioClip[] heavyTrackClips;

    [Header("Adaptive music setup")]
    [Space]
    [SerializeField] private float fadeTime = 2.0f;
    [SerializeField] private AnimationCurve fadeCurve;
#pragma warning restore

    private float coroutineTime = 0.0f;



    private void OnEnable()
    {
        RhythmController.Instance.OnComboStart += FadeToLight;
        RhythmController.Instance.OnRageModeStart += FadeToHeavy;
        RhythmController.Instance.OnRageModeEnd += FadeToLight;
        RhythmController.Instance.OnComboEnd += FadeToNone;
    }

    private void OnDisable()
    {
        RhythmController.Instance.OnComboStart -= FadeToLight;
        RhythmController.Instance.OnRageModeStart -= FadeToHeavy;
        RhythmController.Instance.OnRageModeEnd -= FadeToLight;
        RhythmController.Instance.OnComboEnd -= FadeToNone;
    }

    private void FadeToNone()
    {
        StopAllCoroutines();
        StartCoroutine(FadeToNoneCoroutine());
    }

    private void FadeToLight()
    {
        StopAllCoroutines();
        StartCoroutine(FadeToLightCoroutine());
    }

    private void FadeToHeavy()
    {
        StopAllCoroutines();
        StartCoroutine(FadeToHeavyCoroutine());
    }

    private IEnumerator FadeToNoneCoroutine()
    {
        coroutineTime = 0.0f;
        while (coroutineTime <= fadeTime)
        {
            yield return new WaitForEndOfFrame();

            heavyTrack.volume = Mathf.Clamp01(heavyTrack.volume - fadeCurve.Evaluate(coroutineTime / fadeTime));
            lightTrack.volume = Mathf.Clamp01(lightTrack.volume - fadeCurve.Evaluate(coroutineTime / fadeTime));

            coroutineTime += Time.deltaTime;
        }
    }

    private IEnumerator FadeToLightCoroutine()
    {
        coroutineTime = 0.0f;
        while (coroutineTime <= fadeTime)
        {
            yield return new WaitForEndOfFrame();

            heavyTrack.volume = Mathf.Clamp01(heavyTrack.volume - fadeCurve.Evaluate(coroutineTime / fadeTime));
            lightTrack.volume = Mathf.Clamp01(lightTrack.volume + fadeCurve.Evaluate(coroutineTime / fadeTime));

            coroutineTime += Time.deltaTime;
        }
    }

    private IEnumerator FadeToHeavyCoroutine()
    {
        coroutineTime = 0.0f;
        while (coroutineTime <= fadeTime)
        {
            yield return new WaitForEndOfFrame();

            heavyTrack.volume = Mathf.Clamp01(heavyTrack.volume + fadeCurve.Evaluate(coroutineTime / fadeTime));
            lightTrack.volume = Mathf.Clamp01(lightTrack.volume - fadeCurve.Evaluate(coroutineTime / fadeTime));

            coroutineTime += Time.deltaTime;
        }
    }
}
