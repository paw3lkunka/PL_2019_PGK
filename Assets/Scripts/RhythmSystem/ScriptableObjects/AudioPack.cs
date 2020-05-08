using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioPack_", menuName = "AudioPacks/AudioPack", order = 3)]
public class AudioPack : ScriptableObject
{
    #region Variables

    public AudioClip[] clips;
    public float[] clipsChances;

    #endregion


    #region Component

    public void InitClipChances()
    {
        if (clipsChances == null)
        {
            clipsChances = new float[clips.Length];
        }

        for (int i = 0; i < clipsChances.Length; ++i)
        {
            clipsChances[i] = Random.Range(0.0f, 1.0f);
        }
    }

    public float GetSumOfClipChances()
    {
        float sum = 0.0f;
        foreach (var chance in clipsChances)
        {
            sum += chance;
        }
        return sum;
    }

    #endregion
}
