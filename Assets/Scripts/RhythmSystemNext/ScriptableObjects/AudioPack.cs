using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioPack", menuName = "AudioPack/BaseAudioPack", order = 1)]
public class AudioPack : ScriptableObject
{
    public AudioClip[] mainClips;
    public AudioClip[] secondaryClips;
}
