using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicPack_", menuName = "AudioPacks/MusicPack", order = 2)]
public class MusicPack : ScriptableObject
{
    #region Variables

    public AudioClip mainClipLight;
    public AudioClip mainClipHeavy;
    public AudioClip[] secondaryClipsLight;
    public AudioClip[] secondaryClipsHeavy;
    public float[] secondaryClipsChances;

    #endregion

    #region Component

    public void InitClipChances()
    {
        if (secondaryClipsChances == null)
        {
            secondaryClipsChances = new float[secondaryClipsHeavy.Length];
        }

        for (int i = 0; i < secondaryClipsChances.Length; ++i)
        {
            secondaryClipsChances[i] = Random.Range(0.0f, 1.0f);
        }
    }

    public float GetSumOfClipChances()
    {
        float sum = 0.0f;
        foreach (var chance in secondaryClipsChances)
        {
            sum += chance;
        }
        return sum;
    }

    #endregion
}
