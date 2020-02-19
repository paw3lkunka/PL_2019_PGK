using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RhythmModule : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private bool playOffbeat = false;
    [SerializeField] private AudioClip countupHiHat;
    [SerializeField] private AudioClip countupKick;
#pragma warning restore

    private AudioSource rhythmSource;

    private void Awake()
    {
        rhythmSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        AudioTimeline.Instance.OnBeat += PlayCount;
    }

    private void OnDisable()
    {
        AudioTimeline.Instance.OnBeat -= PlayCount;
    }

    public void PlayCount(bool isMain)
    {
        rhythmSource.PlayOneShot(countupKick);

        if (playOffbeat && isMain)
        {
            StartCoroutine(OffbeatHat());
        }
    }

    private IEnumerator OffbeatHat()
    {
        yield return new WaitForSeconds((float)(60.0d / AudioTimeline.Instance.SongBpm) / 2.0f);
        rhythmSource.PlayOneShot(countupHiHat);
    }
}
