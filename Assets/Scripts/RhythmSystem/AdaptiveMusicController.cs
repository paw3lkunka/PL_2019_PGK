using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveMusicController : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private RhythmController rhythmController;
    [Space]
    [SerializeField] private AudioSource lightMusic;
    [SerializeField] private AudioSource heavyMusic;
    [Space]
    [SerializeField] private float fadeSpeed = 0.1f;
#pragma warning restore

    private int lastFrameCombo = 0;
    private bool lastFrameRageStatus = false;

    private int thisFrameCombo = 0;
    private bool thisFrameRageStatus = false;

    private void LateUpdate()
    {
        thisFrameCombo = rhythmController.GetCurrentCombo();
        thisFrameRageStatus = rhythmController.IsInRageMode();

        if (thisFrameCombo > 0)
        {
            if (thisFrameRageStatus)
            {
                heavyMusic.volume = Mathf.Clamp01(heavyMusic.volume + fadeSpeed);
                lightMusic.volume = Mathf.Clamp01(lightMusic.volume - fadeSpeed);
            }
            else
            {
                heavyMusic.volume = Mathf.Clamp01(heavyMusic.volume - fadeSpeed);
                lightMusic.volume = Mathf.Clamp01(lightMusic.volume + fadeSpeed);
            }
        }
        else
        {
            heavyMusic.volume = Mathf.Clamp01(heavyMusic.volume - fadeSpeed);
            lightMusic.volume = Mathf.Clamp01(lightMusic.volume - fadeSpeed);
        }
    }


}
