using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioPack", menuName = "AudioPack/BaseAudioPack", order = 1)]
public class AudioPack : ScriptableObject
{
    #region Variables

    public AudioClip mainClip;
    public AudioClip[] secondaryClips;

    public float[] secondaryClipsChances;

    #endregion

    #region MonoBehaviour



    #endregion

    #region Component

    public void InitClipChances()
    {
        if (secondaryClipsChances == null)
        {
            secondaryClipsChances = new float[secondaryClips.Length];
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
