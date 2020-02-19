using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveMusicController : MonoBehaviour
{
    #region Variables

#pragma warning disable
    [SerializeField] private AudioSource lightMusic;
    [SerializeField] private AudioSource heavyMusic;
    [Space]
    [SerializeField] private float fadeTime = 2.0f;
    [SerializeField] private AnimationCurve fadeCurve;
#pragma warning restore

    private float coroutineTime = 0.0f;

    #endregion

    #region MonoBehaviour

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

    #endregion

    #region Component

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

            heavyMusic.volume = Mathf.Clamp01(heavyMusic.volume - fadeCurve.Evaluate(coroutineTime / fadeTime));
            lightMusic.volume = Mathf.Clamp01(lightMusic.volume - fadeCurve.Evaluate(coroutineTime / fadeTime));

            coroutineTime += Time.deltaTime;
        }
    }

    private IEnumerator FadeToLightCoroutine()
    {
        coroutineTime = 0.0f;
        while (coroutineTime <= fadeTime)
        {
            yield return new WaitForEndOfFrame();

            heavyMusic.volume = Mathf.Clamp01(heavyMusic.volume - fadeCurve.Evaluate(coroutineTime / fadeTime));
            lightMusic.volume = Mathf.Clamp01(lightMusic.volume + fadeCurve.Evaluate(coroutineTime / fadeTime));

            coroutineTime += Time.deltaTime;
        }
    }

    private IEnumerator FadeToHeavyCoroutine()
    {
        coroutineTime = 0.0f;
        while (coroutineTime <= fadeTime)
        {
            yield return new WaitForEndOfFrame();

            heavyMusic.volume = Mathf.Clamp01(heavyMusic.volume + fadeCurve.Evaluate(coroutineTime / fadeTime));
            lightMusic.volume = Mathf.Clamp01(lightMusic.volume - fadeCurve.Evaluate(coroutineTime / fadeTime));

            coroutineTime += Time.deltaTime;
        }
    }

    #endregion
}
