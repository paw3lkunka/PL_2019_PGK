using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    //IT'S NOT WORKING, DO SMTH ABOUT IT!!!111
    public void MuteMusic()
    {
        var sources = GetComponentsInChildren<AudioSource>();
        foreach(var src in sources) 
        {
            StartCoroutine(MuteCouroutine(src));
        }
        
    }

    private IEnumerator MuteCouroutine(AudioSource src)
    {
        while(src.volume > 0.01f)
        {
            src.volume -= 0.01f;
            yield return new WaitForFixedUpdate();
        }
        Destroy(src.gameObject);
    }
}
