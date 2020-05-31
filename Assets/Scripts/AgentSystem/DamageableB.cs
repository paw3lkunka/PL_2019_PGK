using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableB : Damageable, IBoostable
{

    [field: SerializeField, GUIName("DefenseBoost")]
    public float DefenseBoost { get; set; }

    [field: SerializeField, GUIName("IsBoosted"), GUIReadOnly]
    public bool IsBoosted { get; set; }

    protected override float CalculateDamage(float hitPoints) => Mathf.Clamp(hitPoints - (IsBoosted ? DefenseBoost : DefenseBase), 0, Health);

}
