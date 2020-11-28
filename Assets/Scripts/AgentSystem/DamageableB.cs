using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableB : Damageable, IBoostable
{

    [field: SerializeField, GUIName("DefenseBoost")]
    public float DefenseBoost { get; set; }

    [field: SerializeField, GUIName("DefenseDecrese")]
    public float DefenseDecrese { get; set; }

    [SerializeField]
    private BoostableState bState;
    public BoostableState BState
    { 
        get => bState; 
        set
        {
            switch (value)
            {
                case BoostableState.normal:
                    bState = value;
                    break;

                case BoostableState.boosted:
                    if (CanBeBoosted)
                        bState = value;
                    break;

                case BoostableState.decresed:
                    if (CanBeDecresed)
                        bState = value;
                    break;
            }
        }
    }

    public bool IsBoosted => BState == BoostableState.boosted;
    public bool IsDecresed => BState == BoostableState.decresed;

    [field: SerializeField, InspectorName("CanBeBoosted")]
    public bool CanBeBoosted { get; set; }

    [field: SerializeField, InspectorName("CanBeDecresed")]
    public bool CanBeDecresed { get; set; }

    protected override float CalculateDamage(float hitPoints)
    {
        float defence = 0;

        switch (BState)
        {
            case BoostableState.normal:
                defence = DefenseBase;
                break;

            case BoostableState.boosted:
                defence = DefenseBoost;
                break;

            case BoostableState.decresed:
                defence = DefenseDecrese;
                break;
        }

        return Mathf.Clamp(hitPoints - defence, 0, Health);
    }

}
