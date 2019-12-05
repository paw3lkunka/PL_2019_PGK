using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AdaptiveMusicSequencer : MonoBehaviour
{
#pragma warning disable

#pragma warning restore

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        AudioSettings.dspTime;
    }

}
