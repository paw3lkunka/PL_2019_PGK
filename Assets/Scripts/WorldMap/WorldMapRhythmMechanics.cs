using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Moveable))]
public class WorldMapRhythmMechanics : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private float healthAddPerCultist = 0.1f;
    [SerializeField] private float waterSubtractPerCultist = 0.3f;
    [SerializeField] private float faithSubtractPerCultist = 0.1f;
#pragma warning restore

    private MoveableB moveable;

    private void OnEnable()
    {
        AudioTimeline.Instance.OnBeatHit += Instance_OnBeatHit;
        moveable = GetComponent<Moveable>() as MoveableB;
    }

    private void Instance_OnBeatHit(BeatState beatState, int beatNumber, bool primaryInteraction)
    {
        if (beatState == BeatState.Good || beatState == BeatState.Great || beatState == BeatState.Perfect)
        {
            if (!primaryInteraction)
            {
                moveable.Stop();
                moveable.BState = BoostableState.normal;

                foreach (var cultist in GameplayManager.Instance.cultistInfos)
                {
                    if (cultist.HP < cultist.HP.Max)
                    {
                        cultist.HP += healthAddPerCultist;
                        GameplayManager.Instance.Water -= waterSubtractPerCultist;
                        GameplayManager.Instance.Faith -= faithSubtractPerCultist;
                    }
                }
            }
            else
            {
                moveable.BState = BoostableState.boosted;
            }
        }
        else
        {
            moveable.BState = BoostableState.normal;
        }
    }
}
