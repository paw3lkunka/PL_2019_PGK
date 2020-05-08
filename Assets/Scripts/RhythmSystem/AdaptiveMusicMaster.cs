using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Randomizer 
{ 
    Random, 
    Roulette 
}

public class AdaptiveMusicMaster : MonoBehaviour
{
    #region Variables

#pragma warning disable
    [Header("Setup")]
    [SerializeField] private float fadeSpeed;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource drumsSource;
    [SerializeField] private AudioSource lightMusicSource;
    [SerializeField] private AudioSource heavyMusicSource;

    [Header("Adaptive music variation variables")]
    [SerializeField] [Range(0, 1)] private float nextRhythmPackChance;
    [SerializeField] [Range(0, 1)] private float internalRhythmClipVariation;
    [SerializeField] [Range(0, 1)] private float nextMusicPackChance;
    [SerializeField] [Range(0, 1)] private float internalMusicClipVariation;

    [Header("Music packs for drums and music")]
    [SerializeField] private AudioPack transitionStings;
    [SerializeField] private DrumPack[] drumPacks;
    [SerializeField] private MusicPack[] musicPacks;

    [Header("Options")]
    [SerializeField] private Randomizer packRandomizer = Randomizer.Roulette;
    [SerializeField] private Randomizer clipRandomizer = Randomizer.Random;
    [SerializeField] private bool stingsEnabled = false;
    [SerializeField] private bool drumsEnabled = true;
    [SerializeField] private bool musicEnabled = true;
#pragma warning restore

    private float[] drumPacksChances;
    private float[] musicPacksChances;

    private DrumPack currentDrumPack;
    private MusicPack currentMusicPack;

    public AudioClip CurrentDrumClip { get; private set; }
    public System.Tuple<AudioClip, AudioClip> CurrentMusicClip { get; private set; } // ! First - light, Second - heavy

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        drumPacksChances = new float[drumPacks.Length];
        musicPacksChances = new float[musicPacks.Length];

        currentDrumPack = drumPacks[0];
        currentMusicPack = musicPacks[0];

        for (int i = 0; i < drumPacksChances.Length; ++i)
        {
            drumPacksChances[i] = Random.Range(0.0f, 1.0f);
            drumPacks[i].InitClipChances();
        }

        for (int i = 0; i < musicPacksChances.Length; ++i)
        {
            musicPacksChances[i] = Random.Range(0.0f, 1.0f);
            musicPacks[i].InitClipChances();
        }
    }

    private void OnEnable()
    {
        AudioTimeline.Instance.OnBeat += PlayNext;
        AudioTimeline.Instance.OnBeatFail += StopPlayback;
        AudioTimeline.Instance.OnSequencePause += StopPlayback;
        RhythmMechanics.Instance.OnRageStart += FadeToHeavy;
        RhythmMechanics.Instance.OnRageStop += FadeToLight;
    }

    private void OnDisable()
    {
        AudioTimeline.Instance.OnBeat -= PlayNext;
        AudioTimeline.Instance.OnBeatFail -= StopPlayback;
        AudioTimeline.Instance.OnSequencePause -= StopPlayback;
        RhythmMechanics.Instance.OnRageStart -= FadeToHeavy;
        RhythmMechanics.Instance.OnRageStop -= FadeToLight;
    }

    #endregion

    #region Component

    private void FadeToLight()
    {
        StopAllCoroutines();
        //Debug.Log("Fade to light");
        StartCoroutine(FadeToLightCoroutine());
    }

    private void FadeToHeavy()
    {
        StopAllCoroutines();
        //Debug.Log("Fade to heavy");
        StartCoroutine(FadeToHeavyCoroutine());
    }

    private IEnumerator FadeToLightCoroutine()
    {
        while(lightMusicSource.volume < 0.99f)
        {
            lightMusicSource.volume = Mathf.Lerp(lightMusicSource.volume, 1.0f, Time.deltaTime * fadeSpeed);
            heavyMusicSource.volume = Mathf.Lerp(heavyMusicSource.volume, 0.0f, Time.deltaTime * fadeSpeed);
            yield return new WaitForEndOfFrame();
        }
        lightMusicSource.volume = 1.0f;
        heavyMusicSource.volume = 0.0f;
    }

    private IEnumerator FadeToHeavyCoroutine()
    {
        while(heavyMusicSource.volume < 0.99f)
        {
            heavyMusicSource.volume = Mathf.Lerp(heavyMusicSource.volume, 1.0f, Time.deltaTime * fadeSpeed);
            lightMusicSource.volume = Mathf.Lerp(lightMusicSource.volume, 0.0f, Time.deltaTime * fadeSpeed);
            yield return new WaitForEndOfFrame();
        }
        heavyMusicSource.volume = 1.0f;
        lightMusicSource.volume = 0.0f;
    }

    private void PlayNext(bool isMain)
    {
        if (isMain && AudioTimeline.Instance.TimelineState == TimelineState.Playing)
        {
            RandomizeClips();

            if (drumsEnabled && CurrentDrumClip != null)
            {
                drumsSource.PlayOneShot(CurrentDrumClip);
            }

            if (musicEnabled && CurrentMusicClip.Item1 != null && CurrentMusicClip.Item2 != null &&
                RhythmMechanics.Instance.Combo > 0)
            {
                lightMusicSource.PlayOneShot(CurrentMusicClip.Item1);
                heavyMusicSource.PlayOneShot(CurrentMusicClip.Item2);
            }

        }
    }

    private void StopPlayback(bool keepCombo)
    {
        StopPlayback();
    }

    private void StopPlayback()
    {
        drumsSource.Stop();
        heavyMusicSource.Stop();
        lightMusicSource.volume = 1.0f;
        heavyMusicSource.volume = 0.0f;
    }

    private void RandomizeClips()
    {
        // ----- PACK RANDOMIZATION -----------------------------------------------------
        // Randomize if the next DRUMS bar should use next internal variation of the pack
        if (Random.Range(0.0f, 1.0f) < nextRhythmPackChance)
        {
            switch (packRandomizer)
            {
                case Randomizer.Random:
                    // Pure random drum pack selection
                    currentDrumPack = drumPacks[Random.Range(0, drumPacks.Length)];
                    break;
                case Randomizer.Roulette:
                    // Roulette pack selection
                    currentDrumPack = drumPacks[RouletteAlgorithm(ref drumPacksChances)];
                    break;
            }
        }
        // Randomize if the next MUSIC bar should use next internal variation of the pack
        if (Random.Range(0.0f, 1.0f) < nextMusicPackChance)
        {
            switch (packRandomizer)
            {
                case Randomizer.Random:
                    currentMusicPack = musicPacks[Random.Range(0, musicPacks.Length)];
                    break;
                case Randomizer.Roulette:
                    currentMusicPack = musicPacks[RouletteAlgorithm(ref musicPacksChances)];
                    break;
            }
        }

        // ----- CLIP RANDOMIZATION -----------------------------------------------------
        if (Random.Range(0.0f, 1.0f) < internalRhythmClipVariation)
        {
            switch (clipRandomizer)
            {
                case Randomizer.Random:
                    CurrentDrumClip = currentDrumPack.secondaryClips[Random.Range(0, currentDrumPack.secondaryClips.Length)];
                    break;
                case Randomizer.Roulette:
                    CurrentDrumClip = currentDrumPack.secondaryClips[RouletteAlgorithm(ref currentDrumPack.secondaryClipsChances)];
                    break;
            }
        }
        else
        {
            CurrentDrumClip = currentDrumPack.mainClip;
        }

        if (Random.Range(0.0f, 1.0f) < internalMusicClipVariation)
        {
            int index = 0;
            switch (clipRandomizer)
            {
                case Randomizer.Random:
                    index = Random.Range(0, currentMusicPack.secondaryClipsHeavy.Length);
                    break;

                case Randomizer.Roulette:
                    index = RouletteAlgorithm(ref currentMusicPack.secondaryClipsChances);
                    break;
            }

            CurrentMusicClip = new System.Tuple<AudioClip, AudioClip>(currentMusicPack.secondaryClipsLight[index], currentMusicPack.secondaryClipsHeavy[index]);
        }
        else
        {
            CurrentMusicClip = new System.Tuple<AudioClip, AudioClip>(currentMusicPack.mainClipLight, currentMusicPack.mainClipHeavy);
        }
    }

    private int RouletteAlgorithm(ref float[] chances)
    {
        int output = 0;
        bool finished = false;
        float waveDecreaseRatio = Random.Range(0.0f, 0.5f);
        float waveIncreaseRatio = waveDecreaseRatio / (chances.Length - 1);
        float rand = Random.Range(0.0f, 1.0f);

        for (int i = 0; i < chances.Length; ++i)
        {
            if (rand < chances[i] && !finished)
            {
                finished = true;
                // Roulette random drum pack selection
                output = i;
                chances[i] -= waveDecreaseRatio;
            }
            else
            {
                rand -= chances[i];
                chances[i] += waveIncreaseRatio;
            }
        }

        return output;
    }

    #endregion
}
