using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Moveable), typeof(IAttack), typeof(Detection))]
public class BehaviourChase : BehaviourRandom
{
    [field: SerializeField, GUIName("ChaseTarget"), GUIReadOnly]
    public bool ChaseTarget { get; protected set; }

    private Detection detector;

    private Vector3? target = null;
    private Vector3? targetOld = null;

    #region MonoBehaviour
    protected override void Awake()
    {
        detector = GetComponent<Detection>();
        base.Awake();
    }

    private void Update()
    {
        target = detector.Func();

        if (target != null && targetOld == null)
        {
            StopAllCoroutines();
            ChaseTarget = true;

            if (target != targetOld)
            {
                Vector3 t = (Vector3)target;
                moveable.Go(t);
            }
        }
        else if (target == null && targetOld != null)
        {
            StartCoroutine(ChangeTargetRoutine());
            ChaseTarget = false;
        }

        targetOld = target;
    }
    #endregion
}
