using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingB : Shooting
{

    [field: SerializeField, GUIName("DamageBoost")]
    public float DamageBoost { get; set; }

    [field: SerializeField, GUIName("RangeBoost")]
    public float RangeBoost { get; set; }

    [field: SerializeField, GUIName("IntervalMultiplierBoost")]
    public float RangeMultiplierBoost { get; set; }

    [field: SerializeField, GUIName("IsBoosted")]
    public bool IsBoosted { get; set; }

    protected override Projectile CreateProjectile()
    {
        if(IsBoosted)
        {
            var projectile = Instantiate(projectilePrefab).GetComponent<Projectile>();
            projectile.damageMin *= DamageBoost;
            projectile.damageMax *= DamageBoost;
            projectile.range *= RangeMultiplierBoost;
            return projectile;
        }
        else
        {
            return base.CreateProjectile();
        }
    }
}
