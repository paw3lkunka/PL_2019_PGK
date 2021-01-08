using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingB : Shooting, IBoostable
{
    [field: Header("B State")]

    [SerializeField]
    private BoostableState bState;

    [field: SerializeField, GUIName("CanBeBoosted")]
    public bool CanBeBoosted { get; set; }

    [field: SerializeField, GUIName("CanBeDecresed")]
    public bool CanBeDecresed { get; set; }


    [field: Header("Boosted")]

    [field: SerializeField, GUIName("DamageBoost")]
    public float DamageBoostedMultiplier { get; set; } = 1;

    [field: SerializeField, GUIName("RangeMultiplierBoost")]
    public float RangeMultiplierBoost { get; set; } = 1;

    [field: SerializeField, GUIName("IntervalBoost")]
    public float IntervalBoost { get; set; } = 1;

    [field: Header("Decresed")]

    [field: SerializeField, GUIName("DamageDecrese")]
    public float DamageDecresedMultiplier { get; set; } = 1;

    [field: SerializeField, GUIName("RangeMultiplierDecrese")]
    public float RangeMultiplierDecrese { get; set; } = 1;

    [field: SerializeField, GUIName("IntervalDecrese")]
    public float IntervalDecrese { get; set; } = 1;


    public override float Interval 
    {
        get
        {
            switch (bState)
            {
                case BoostableState.normal:
                    return IntervalBase;
                case BoostableState.boosted:
                    return IntervalBoost;
                case BoostableState.decresed:
                    return IntervalDecrese;
                default:
                    throw new System.NotImplementedException();
            }
        }
    }


    public bool IsDecresed => BState == BoostableState.boosted;

    public bool IsBoosted => BState == BoostableState.decresed;

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


    protected override Projectile CreateProjectile()
    {
        if (BState == BoostableState.normal)
        {
            return base.CreateProjectile();
        }
        else
        {
            var projectile = Instantiate(
                ApplicationManager.Instance.PrefabDatabase.projectile,
                transform.position,
                Quaternion.identity
            ).GetComponent<Projectile>();

            if (IsBoosted)
            {
                projectile.damageMin *= DamageBoostedMultiplier;
                projectile.damageMax *= DamageBoostedMultiplier;
                projectile.range *= RangeMultiplierBoost;
                SetLayerMask(projectile);
                return projectile;
            }
            else
            {   projectile.damageMin *= DamageDecresedMultiplier;
                projectile.damageMax *= DamageDecresedMultiplier;
                projectile.range *= RangeMultiplierDecrese;
                SetLayerMask(projectile);
                return projectile;
            }
        }
    }
}
