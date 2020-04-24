﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [Flags]
    public enum Flags
    {
        canBeHeald = 0b0000_0001,
        canBeDamaged = 0b0000_0010,
    }

    public Flags flags;

    public Resource health;

    [field: SerializeField, GUIName("Defence")]
    public float DefenseBase {get; set;}

    /// <summary>
    /// Heal agent with given amount of hp
    /// </summary>
    /// <param name="hp">health to add</param>
    void Heal(float hp)
    {
        if( (flags & Flags.canBeHeald) != 0 )
        {
            health.Set(hp);
        }
    }

    /// <summary>
    /// Deal damage. real damage = clamp( hit points - defense, 0, health). 
    /// </summary>
    /// <param name="hitPoints">Force of attack.</param>
    /// <returns>Real damage.</returns>
    public float Damage(float hitPoints)
    {
        if ((flags & Flags.canBeDamaged) != 0)
        {
            float realDamage = CalculateDamage(hitPoints);
            health -= realDamage;
            return realDamage;
        }
        else
        {
            return 0;
        }
    }

    protected virtual float CalculateDamage(float hitPoints) => Mathf.Clamp(hitPoints - DefenseBase, 0, health);

    #region Mono Behaviour

    private void LateUpdate()
    {
        if(health < 0)
        {
            Destroy(gameObject);
        }
    }


    #endregion

}
