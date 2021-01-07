using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class SteamExplosion : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private ParticleSystem bigExplosion;
    [SerializeField] private ParticleSystem smallExplosion;
#pragma warning restore

    private void OnEnable()
    {
        if (AudioTimeline.Instance)
        {
            //AudioTimeline.Instance.OnBeatFail += ExplosionHandler;
        }
    }

    private void OnDisable()
    {
        if (AudioTimeline.Instance)
        {
            //AudioTimeline.Instance.OnBeatFail -= ExplosionHandler;
        }
    }

    //private void ExplosionHandler()
    //{
    //    if (LocationManager.Instance.CanStun)
    //    {
    //        bigExplosion.Play();
    //    }
    //    else
    //    {
    //        smallExplosion.Play();
    //    }
    //}
}
