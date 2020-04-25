using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourUserInput : MonoBehaviour, IBehaviour
{
    Vector3 cursor;

    public void UpdateTarget(Vector3? target)
    {
        throw new System.NotImplementedException();
    }

    #region MonoBehaviour

    private void Awake()
    {
        //TODO - position from group
        cursor = transform.position;
    }

    private void FixedUpdate()
    {
        //TODO - position from group
        cursor = CombatCursorManager.Instance.MainCursor.transform.position;

    }

    #endregion
}
