using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultLeader : MonoBehaviour
{
    private Moveable moveable;

    public Vector2 speedMinMax;

    #region MonoBehaviour
    private void Awake()
    {
        LocationManager.Instance.cultLeader = this;
        moveable = GetComponent<Moveable>();
    }

    private void Update()
    {
        float faith = GameplayManager.Instance.Faith.Normalized;

        moveable.SpeedBase = Mathf.Lerp(speedMinMax.x, speedMinMax.y, faith);
    }

    #endregion
}
