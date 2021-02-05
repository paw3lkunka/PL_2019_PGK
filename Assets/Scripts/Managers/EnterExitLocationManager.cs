using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterExitLocationManager : Singleton<EnterExitLocationManager, AllowLazyInstancing>
{
    public float enterExitTime = 2.0f;

    public float EnterExitProgress { get; private set; }
    public float EnterExitProgressNormalized => EnterExitProgress / enterExitTime;
    public bool IsEnteringExiting { get; set; }

    protected override void Awake()
    {
        _persistent = false;
        base.Awake();
    }

    private void Update()
    {
        if (IsEnteringExiting)
        {
            EnterExitProgress += Time.deltaTime;
        }
        else
        {
            EnterExitProgress -= Time.deltaTime;
        }

        EnterExitProgress = Mathf.Clamp(EnterExitProgress, 0.0f, enterExitTime);
    }
}
