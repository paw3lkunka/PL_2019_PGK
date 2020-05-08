using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    private Detection detection;
    private IAttack attack;
    private IBehaviour behaviour;

    #region MonoBehaviour

    private void Awake()
    {
        detection = GetComponent<Detection>();
        attack = GetComponent<IAttack>();
        behaviour = GetComponent<IBehaviour>();
    }

    private void FixedUpdate()
    {
        Vector3? target = detection?.Func();

        behaviour?.UpdateTarget(target);

        if(attack != null)
        {
            if (target != null)
            {
                attack.Attack(target.Value);
            }
            else
            {
                attack.HoldFire();
            }
        }

    }

    #endregion
}
