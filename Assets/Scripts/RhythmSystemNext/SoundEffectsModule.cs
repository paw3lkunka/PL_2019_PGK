using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectsModule : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private AudioClip goodBeatHitSound;
    [SerializeField] private AudioClip greatBeatHitSound;
    [SerializeField] private AudioClip badBeatHitSound;
    [SerializeField] private AudioClip failSound;
#pragma warning restore

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        AudioTimeline.Instance.OnSequenceReset += PlayFailSound;
    }

    private void OnDisable()
    {
        AudioTimeline.Instance.OnSequenceReset -= PlayFailSound;
    }

    private void PlayFailSound()
    {
        audioSource.PlayOneShot(failSound);
    }
}
