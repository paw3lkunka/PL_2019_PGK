using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectsModule : MonoBehaviour
{
    #region Variables

#pragma warning disable
    [SerializeField] private AudioClip goodBeatHitSound;
    [SerializeField] private AudioClip greatBeatHitSound;
    [SerializeField] private AudioClip badBeatHitSound;
    [SerializeField] private AudioClip failSound;
#pragma warning restore

    private AudioSource audioSource;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        AudioTimeline.Instance.OnSequenceReset += PlayFailSound;
        AudioTimeline.Instance.OnBeatHit += PlayBeatHitSound;
    }

    private void OnDisable()
    {
        AudioTimeline.Instance.OnSequenceReset -= PlayFailSound;
        AudioTimeline.Instance.OnBeatHit -= PlayBeatHitSound;
    }

    #endregion

    #region Component

    private void PlayBeatHitSound(BeatState beatState, int beatNumber, bool primaryInteraction)
    {
        switch (beatState)
        {
            case BeatState.None:
            case BeatState.Bad:
                if (badBeatHitSound != null)
                    audioSource.PlayOneShot(badBeatHitSound);
                break;
            case BeatState.Good:
                if (goodBeatHitSound != null)
                    audioSource.PlayOneShot(goodBeatHitSound);
                break;
            case BeatState.Great:
            case BeatState.Perfect:
                if (greatBeatHitSound != null)
                    audioSource.PlayOneShot(greatBeatHitSound);
                break;
        }
    }

    private void PlayFailSound()
    {
        audioSource.PlayOneShot(failSound);
    }

    #endregion
}
