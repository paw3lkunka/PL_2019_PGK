using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RhythmTest : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private float songBpm;
    [SerializeField] private GameObject rhythmIndicator;
    [SerializeField] private GameObject rhythmIndicatorCoroutine;
    [SerializeField] private GameObject goodRhythmIndicator;
    [SerializeField] private float timingAccuracy;
#pragma warning restore

    private AudioSource audioSource;

    private float beatInterval;
    private float timeSinceLastBeat;

    private void Start()
    {
        beatInterval = 60.0f / songBpm;
        Debug.Log("beat interval: " + beatInterval);
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        StartCoroutine(BeatCoroutine());
    }

    private void Update()
    {
        if (timeSinceLastBeat % beatInterval < timingAccuracy)

        if (timeSinceLastBeat > beatInterval - timingAccuracy && Input.anyKeyDown)
        {
            goodRhythmIndicator.SetActive(true);
        }
        else if (timeSinceLastBeat >= beatInterval)
        {
            rhythmIndicator.SetActive(true);
            timeSinceLastBeat = 0;
        }
        else
        {
            rhythmIndicator.SetActive(false);
            goodRhythmIndicator.SetActive(false);
        }
        timeSinceLastBeat += Time.deltaTime;
        Debug.Log("Time since last beat: " + timeSinceLastBeat);
    }

    private IEnumerator BeatCoroutine()
    {
        while (true)
        {
            rhythmIndicatorCoroutine.SetActive(false);
            yield return new WaitForSeconds(beatInterval);
            rhythmIndicatorCoroutine.SetActive(true);
            yield return new WaitForEndOfFrame();
        }
    }
}
