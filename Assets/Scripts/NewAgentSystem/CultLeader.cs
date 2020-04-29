using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultLeader : MonoBehaviour
{
    private Damageable damageable;


    #region MonoBehaviour
    private void Awake()
    {
        damageable = GetComponent<Damageable>();
        damageable.flags &= ~Damageable.Flags.canBeDamaged;
    }

    private void Update()
    {
        if (CombatSceneManager.Instance.ourCrew.Count == 0)
        {
            CombatSceneManager.Instance.ourCrew.Add(damageable);
            damageable.flags |= Damageable.Flags.canBeDamaged;
        }
    }

    private void OnDisable()
    {
        CombatSceneManager.Instance.ourCrew.Remove(damageable);
    }

    #endregion

}
