using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<UnityEngine.UI.Slider>().value = AudioListener.volume;
    }

    public void AdjustVolume(float newVolume)
    {
        AudioListener.volume = newVolume;
    }
}
