using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MoveableB : Moveable, IBoostable
{
    [field: SerializeField, GUIName("SpeedBoosted")]
    public float SpeedBoosted { get; set; }

    [field: SerializeField, GUIName("IsBoosted")]
    public bool IsBoosted { get; set; }

    #region MonoBehaviour
    private new void OnValidate()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected new void Awake()
    {
        base.Awake();
    }
    protected new void Update()
    {
        navMeshAgent.speed = IsBoosted ? SpeedBoosted : SpeedBase;
    }

    #endregion
}
