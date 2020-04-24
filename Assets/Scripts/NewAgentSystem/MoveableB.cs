using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MoveableB : Moveable, IBoostable
{

    [field: SerializeField, GUIName("SpeedBase")]
    public override float SpeedBase { get; set; }

    [field: SerializeField, GUIName("SpeedBoosted")]
    public float SpeedBoosted { get; set; }

    public bool IsBoosted 
    {
        get => isBoosted;
        set
        {
            if (isBoosted = value)
            {
                navMeshAgent.speed = SpeedBoosted;
            }
            else
            {
                navMeshAgent.speed = SpeedBase;
            }
        }
    }
    [SerializeField]
    private bool isBoosted;

    #region MonoBehaviour
    private new void OnValidate()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        IsBoosted = isBoosted;
    }

    protected new void Awake()
    {
        base.Awake();
    }

    #endregion
}
