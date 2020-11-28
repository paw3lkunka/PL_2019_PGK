using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingB : Shooting, IBoostable
{
    [field: Header("Boosted")]

    [field: SerializeField, GUIName("DamageBoost")]
    public float DamageBoost { get; set; } = 1;

    [field: SerializeField, GUIName("DamageDecrese")]
    public float DamageDecrese { get; set; } = 1;

    [field: SerializeField, GUIName("RangeBoost")]
    public float RangeBoost { get; set; } = 1;

    [field: SerializeField, GUIName("IntervalMultiplierBoost")]
    public float RangeMultiplierBoost { get; set; } = 1;

    [field: SerializeField, GUIName("RangeMultiplierDecrese")]
    public float RangeMultiplierDecrese { get; set; } = 1;

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

    [field: SerializeField, GUIName("CanBeBoosted")]
    public bool CanBeBoosted { get; set; }

    [field: SerializeField, GUIName("CanBeDecresed")]
    public bool CanBeDecresed { get; set; }

    public bool IsDecresed => BState == BoostableState.boosted;

    public bool IsBoosted => BState == BoostableState.decresed;

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
                projectile.damageMin *= DamageBoost;
                projectile.damageMax *= DamageBoost;
                projectile.range *= RangeMultiplierBoost;
                SetLayerMask(projectile);
                return projectile;
            }
            else
            {   projectile.damageMin *= DamageDecrese;
                projectile.damageMax *= DamageDecrese;
                projectile.range *= RangeMultiplierDecrese;
                SetLayerMask(projectile);
                return projectile;
            }
        }
    }
}
