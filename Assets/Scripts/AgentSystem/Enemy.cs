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

    // CAnnot be on on enable / disable
    private void Start()
    {
        AudioTimeline.Instance.OnBeatFail += EnterStun;
        AudioTimeline.Instance.OnCountupEnd += ExitStun;
    }
    private void OnDestroy()
    {
        AudioTimeline.Instance.OnBeatFail -= EnterStun;
        AudioTimeline.Instance.OnCountupEnd -= ExitStun;
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

    public void EnterStun()
    {
        Debug.Log("STUN ENTER");
        enabled = false;
        attack.HoldFire();
        behaviour.enabled = false;
    }

    public void ExitStun()
    {
        Debug.Log("STUN EXIT");
        enabled = true;
        behaviour.enabled = true;
    }
}
