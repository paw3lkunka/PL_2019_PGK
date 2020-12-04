using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MoveableB : Moveable, IBoostable
{
    [field: SerializeField, GUIName("SpeedBoosted")]
    public float SpeedBoosted { get; set; }

    [field: SerializeField, GUIName("SpeedDecresed")]
    public float SpeedDecresed { get; set; }

    [field: Header("B State")]

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

    [field: SerializeField, InspectorName("CanBeBoosted")]
    public bool CanBeBoosted { get; set; }

    [field: SerializeField, InspectorName("CanBeDecresed")]
    public bool CanBeDecresed { get; set; }

    public bool IsBoosted => bState == BoostableState.boosted;

    public bool IsDecresed => bState == BoostableState.decresed;

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
        switch (bState)
        {
            case BoostableState.normal:
                navMeshAgent.speed = SpeedBase;
                break;

            case BoostableState.boosted:
                navMeshAgent.speed = SpeedBoosted;
                break;

            case BoostableState.decresed:
                navMeshAgent.speed = SpeedDecresed;
                break;
        }
    }

    #endregion
}
