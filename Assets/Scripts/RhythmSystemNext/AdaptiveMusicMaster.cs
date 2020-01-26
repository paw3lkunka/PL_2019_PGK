using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Randomizer { Random, Roulette }

public class AdaptiveMusicMaster : MonoBehaviour
{
#pragma warning disable
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
    [SerializeField] private AudioPack[] drumPacks;
    [SerializeField] private AudioPack[] lightMusicPacks;
    [SerializeField] private AudioPack[] heavyMusicPacks;
    [Header("Options")]
    [SerializeField] private Randomizer packRandomizer = Randomizer.Roulette;
    [SerializeField] private Randomizer clipRandomizer = Randomizer.Random;
    [SerializeField] private bool stingsEnabled = false;
    [SerializeField] private bool drumsEnabled = true;
    [SerializeField] private bool musicEnabled = true;
#pragma warning restore

    private float[] drumPacksChances;
    private float[] lightMusicPacksChances;
    private float[] heavyMusicPacksChances;

    private AudioPack currentDrumPack;
    private AudioPack currentLightMusicPack;
    private AudioPack currentHeavyMusicPack;

    private AudioClip currentDrumClip;
    private AudioClip currentLightMusicClip;
    private AudioClip currentHeavyMusicClip;

    private void OnEnable()
    {
        AudioTimeline.Instance.OnBeat += PlayNext;
    }

    private void OnDisable()
    {
        AudioTimeline.Instance.OnBeat -= PlayNext;
    }

    private void Awake()
    {
        drumPacksChances = new float[drumPacks.Length];
        lightMusicPacksChances = new float[lightMusicPacks.Length];
        heavyMusicPacksChances = new float[heavyMusicPacks.Length];

        currentDrumPack = drumPacks[0];
        //currentLightMusicPack = lightMusicPacks[0];
        currentHeavyMusicPack = heavyMusicPacks[0];

        for (int i = 0; i < drumPacksChances.Length; ++i)
        {
            drumPacksChances[i] = Random.Range(0.0f, 1.0f);
            drumPacks[i].InitClipChances();
        }

        for (int i = 0; i < lightMusicPacksChances.Length; ++i)
        {
            lightMusicPacksChances[i] = Random.Range(0.0f, 1.0f);
            lightMusicPacks[i].InitClipChances();
        }

        for (int i = 0; i < heavyMusicPacksChances.Length; ++i)
        {
            heavyMusicPacksChances[i] = Random.Range(0.0f, 1.0f);
            heavyMusicPacks[i].InitClipChances();
        }
    }

    private void PlayNext(bool isMain)
    {
        if (isMain)
        {
            RandomizeClips();

            if (drumsEnabled && !currentDrumClip.IsRealNull())
            {
                drumsSource.PlayOneShot(currentDrumClip);
            }

            if (musicEnabled &&/* !currentLightMusicClip.IsRealNull() && */!currentHeavyMusicClip.IsRealNull() &&
                AudioTimeline.Instance.Combo > 1)
            {
                //lightMusicSource.PlayOneShot(currentLightMusicClip);
                heavyMusicSource.PlayOneShot(currentHeavyMusicClip);
            }

        }
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
                    currentDrumPack = RouletteAlgorithm(drumPacks, ref drumPacksChances);
                    break;
            }   
        }
        // Randomize if the next MUSIC bar should use next internal variation of the pack
        if (Random.Range(0.0f, 1.0f) < nextMusicPackChance)
        {
            switch (packRandomizer)
            {
                case Randomizer.Random:
                    //currentLightMusicPack = lightMusicPacks[Random.Range(0, lightMusicPacks.Length)];
                    currentHeavyMusicPack = heavyMusicPacks[Random.Range(0, heavyMusicPacks.Length)];
                    break;
                case Randomizer.Roulette:
                    //currentLightMusicPack = RouletteAlgorithm(lightMusicPacks, ref lightMusicPacksChances);
                    currentHeavyMusicPack = RouletteAlgorithm(heavyMusicPacks, ref heavyMusicPacksChances);
                    break;
            }
        }

        // ----- CLIP RANDOMIZATION -----------------------------------------------------
        if (Random.Range(0.0f, 1.0f) < internalRhythmClipVariation)
        {
            switch (clipRandomizer)
            {
                case Randomizer.Random:
                    currentDrumClip = currentDrumPack.secondaryClips[Random.Range(0, currentDrumPack.secondaryClips.Length)];
                    break;
                case Randomizer.Roulette:
                    currentDrumClip = RouletteAlgorithm(currentDrumPack.secondaryClips, ref currentDrumPack.secondaryClipsChances);
                    break;
            }
        }
        else
        {
            currentDrumClip = currentDrumPack.mainClip;
        }

        if (Random.Range(0.0f, 1.0f) < internalMusicClipVariation)
        {
            switch (clipRandomizer)
            {
                case Randomizer.Random:
                    //currentLightMusicClip = currentLightMusicPack.secondaryClips[Random.Range(0, currentLightMusicPack.secondaryClips.Length)];
                    currentHeavyMusicClip = currentHeavyMusicPack.secondaryClips[Random.Range(0, currentHeavyMusicPack.secondaryClips.Length)];
                    break;
                case Randomizer.Roulette:
                    //currentLightMusicClip = RouletteAlgorithm(currentLightMusicPack.secondaryClips, ref currentLightMusicPack.secondaryClipsChances);
                    currentHeavyMusicClip = RouletteAlgorithm(currentHeavyMusicPack.secondaryClips, ref currentHeavyMusicPack.secondaryClipsChances);
                    break;
            }
        }
        else
        {
            //currentLightMusicClip = currentLightMusicPack.mainClip;
            currentHeavyMusicClip = currentHeavyMusicPack.mainClip;
        }
    }

    private T RouletteAlgorithm<T>(T[] packs, ref float[] chances)
    {
        T output = packs[0];
        bool finished = false;
        float waveDecreaseRatio = Random.Range(0.0f, 0.5f);
        float waveIncreaseRatio = waveDecreaseRatio / (chances.Length - 1);
        float rand = Random.Range(0.0f, 1.0f);

        for (int i = 0; i < packs.Length; ++i)
        {
            if (rand < chances[i] && !finished)
            {
                finished = true;
                // Roulette random drum pack selection
                output = packs[i];
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

}
