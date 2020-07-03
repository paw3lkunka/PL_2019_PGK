using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [Flags]
    public enum Flags
    {
        canBeHealed = 0b0000_0001,
        canBeDamaged = 0b0000_0010,
    }

    public Flags flags = Flags.canBeDamaged | Flags.canBeHealed;


    [field: SerializeField, GUIName("Health")]
    public Resource Health { get; private set; } = new Resource(10, 10);

    [field: SerializeField, GUIName("Defence")]
    public float DefenseBase { get; set; } = 0;

    [field: SerializeField, GUIName("ReactToHit")]
    public bool ReactToHit { get; private set; } = false;

    public event Action<float> DamageTaken;
    public event Action Death;


    /// <summary>
    /// Heal agent with given amount of hp
    /// </summary>
    /// <param name="hp">health to add</param>
    public void Heal(float hp)
    {
        if( (flags & Flags.canBeHealed) != 0 )
        {
            Health += hp;
        }
    }

    /// <summary>
    /// Sets value of Health ignoreing in-game logic.
    /// Should not be invoked in gameplay.
    /// </summary>
    /// <param name="value">value to set.</param>
    public void SetHealthForce(float value)
    {
        Health.Set(value);
    }

    /// <summary>
    /// Deal damage. real damage = clamp( hit points - defense, 0, health). 
    /// </summary>
    /// <param name="hitPoints">Force of attack.</param>
    /// <returns>Real damage.</returns>
    public float Damage(IAttack agressor, float hitPoints)
    {
        if (ReactToHit && TryGetComponent<Enemy>(out var enemy))
        {
            enemy.EnterBlindChase(agressor.gameObject.transform.position);
        }

        if ((flags & Flags.canBeDamaged) != 0)
        {
            float realDamage = CalculateDamage(hitPoints);
            Health -= realDamage;

            if( realDamage != 0 && DamageTaken != null)
            {
                DamageTaken(realDamage);
            }

            return realDamage;
        }
        else
        {
            return 0;
        }
    }

    protected virtual float CalculateDamage(float hitPoints) => Mathf.Clamp(hitPoints - DefenseBase, 0, Health);

    #region Mono Behaviour

    private void LateUpdate()
    {
        if(Health <= 0)
        {
            Death?.Invoke();

            if (gameObject.CompareTag("Leader"))
            {
                GameplayManager.Instance.leaderIsDead = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }


    #endregion

}
