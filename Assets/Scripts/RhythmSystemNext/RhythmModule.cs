using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RhythmModule : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private AudioClip countupHiHat;
    [SerializeField] private AudioClip countupKick;
    [SerializeField] private AudioPack[] drumPacks;
#pragma warning restore

    private AudioSource rhythmSource;

    private void Awake()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        rhythmSource = audioSources[0];
        //drumSourceA = audioSources[1];
        //drumSourceB = audioSources[2];
        //drumSourceA.clip = lightDrumTracks[0];
    }

    private void Start()
    {
        AudioTimeline.Instance.OnBeat += PlayHat;
    }

    public void PlayHat(bool isMain)
    {
        if (isMain)
        {
            rhythmSource.PlayOneShot(countupKick);
            StartCoroutine(OffbeatHat());
        }
        else
        {
            rhythmSource.PlayOneShot(countupKick);
        }
    }

    private IEnumerator OffbeatHat()
    {
        yield return new WaitForSeconds((float)(60.0d / AudioTimeline.Instance.SongBpm) / 2.0f);
        rhythmSource.PlayOneShot(countupHiHat);
    }
}
