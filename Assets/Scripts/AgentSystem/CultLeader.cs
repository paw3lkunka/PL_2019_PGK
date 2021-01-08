using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultLeader : MonoBehaviour
{
    private MoveableB moveableB;

    public Vector2 speedMinMax;

    #region MonoBehaviour
    private void Awake()
    {
        LocationManager.Instance.cultLeader = this;
        moveableB = GetComponent<MoveableB>();
    }

    private void Update()
    {
        float faith = GameplayManager.Instance.Faith.Normalized;

        moveableB.SpeedBase = Mathf.Lerp(speedMinMax.x, speedMinMax.y, faith);
    }

    private void OnEnable()
    {
        GameplayManager.Instance.LowFaithLevelStart += OnLowFaithStart;
        GameplayManager.Instance.LowFaithLevelEnd += OnNormalFaithStart;
        GameplayManager.Instance.OverfaithStart += OnOverfaithStart;
        GameplayManager.Instance.OverfaithEnd += OnNormalFaithStart;
    }

    private void OnDisable()
    {
        GameplayManager.Instance.LowFaithLevelStart -= OnLowFaithStart;
        GameplayManager.Instance.LowFaithLevelEnd -= OnNormalFaithStart;
        GameplayManager.Instance.OverfaithStart -= OnOverfaithStart;
        GameplayManager.Instance.OverfaithEnd -= OnNormalFaithStart;
    }

    #endregion

    private void OnOverfaithStart() => moveableB.BState = BoostableState.boosted;
    private void OnLowFaithStart() => moveableB.BState = BoostableState.decresed;
    private void OnNormalFaithStart() => moveableB.BState = BoostableState.normal;

}
