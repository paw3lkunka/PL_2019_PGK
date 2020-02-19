using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayScheduledTest : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audio1;
    public AudioClip audio2;

    private int which = 0;

    public double playMoment = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        AudioTimeline.Instance.OnBeat += PlayAlternating;
    }

    private void OnDisable()
    {
        AudioTimeline.Instance.OnBeat -= PlayAlternating;
    }

    private void PlayAlternating(bool isMain)
    {
        if (isMain)
        {
            if (which == 0)
            {
                audioSource.PlayOneShot(audio1);
                which = 1;
            }
            else if (which == 1)
            {
                audioSource.PlayOneShot(audio2);
                which = 0;
            }
        }
    }

    [ContextMenu("Play Scheduled A")]
    public void PlayScheduledA()
    {
        audioSource.clip = audio1;
        audioSource.PlayScheduled(AudioSettings.dspTime + 3.0d);
    }

    [ContextMenu("Play Scheduled B")]
    public void PlayScheduledB()
    {
        audioSource.clip = audio2;
        audioSource.PlayScheduled(AudioSettings.dspTime + 3.0d);
    }

    [ContextMenu("Play One Shot")]
    public void PlayOneShot()
    {
        audioSource.PlayOneShot(audio1);
    }
    
}
