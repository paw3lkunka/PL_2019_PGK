using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    private Detection detection;
    private IAttack attack;
    private IBehaviour behaviour;
    private Moveable moveable;

    public float blindChaseMatchDistance = 0.5f;
    private bool blindChaseMode;
    private Vector3 blindChaseTarget;

    #region MonoBehaviour

    private void Awake()
    {
        detection = GetComponent<Detection>();
        attack = GetComponent<IAttack>();
        behaviour = GetComponent<IBehaviour>();
        moveable = GetComponent<Moveable>();
    }

    // CAnnot be on on enable / disable
    private void Start()
    {
        AudioTimeline.Instance.OnBeatFail += EnterStun;
    }
    private void OnDestroy()
    {
        AudioTimeline.Instance.OnBeatFail -= EnterStun;
    }

    private void FixedUpdate()
    {
        Vector3? target = detection?.Func();


        if (blindChaseMode && target.HasValue)
        {
            ExitBlindChase();
        }

        behaviour?.UpdateTarget(blindChaseMode ? blindChaseTarget : target);

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

        if (Vector3.Distance(transform.position, blindChaseTarget) < blindChaseMatchDistance)
        {
            ExitBlindChase();
        }

    }

    #endregion

    public void EnterBlindChase(Vector3 target)
    {
        blindChaseTarget = target;
        blindChaseMode = true;
    }

    public void ExitBlindChase()
    {
        blindChaseMode = false;
    }

    public void EnterStun()
    {
        if (LocationManager.Instance.CanStun)
        {
            attack.HoldFire();
            moveable.Stop();

            enabled = false;
            behaviour.enabled = false;

            IEnumerator Routine()
            {
                yield return new WaitForSeconds(1);
                ExitStun();
            }

            StartCoroutine(Routine());
        }
    }

    public void ExitStun()
    {
        enabled = true;
        behaviour.enabled = true;
    }
}
