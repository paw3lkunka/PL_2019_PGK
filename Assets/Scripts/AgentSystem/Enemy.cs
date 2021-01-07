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
        if(AudioTimeline.Instance)
        {
            AudioTimeline.Instance.OnBeatFail -= EnterStun;
        }
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
        GameplayManager.Instance.avoidingFightTimer = 0.0f;
    }

    public void ExitBlindChase()
    {
        blindChaseMode = false;
    }

    public void EnterStun(bool reset)
    {
        if (LocationManager.Instance.CanStun && reset)
        {
            attack.HoldFire();
            moveable.Stop();

            enabled = false;
            behaviour.enabled = false;

            // HACK: Simple way to show that an enemy is stunned
            GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", Color.green);

            IEnumerator Routine()
            {
                float bps = (float)AudioTimeline.Instance.SongBpm / 60.0f;
                int beats = AudioTimeline.Instance.FailedBeatResetOffset + AudioTimeline.Instance.BeatsPerBar;

                yield return new WaitForSeconds(beats / bps);
                ExitStun();
            }

            StartCoroutine(Routine());
        }
    }

    public void ExitStun()
    {
        // HACK: Simple way to show that an enemy is stunned
        GetComponentInChildren<MeshRenderer>().material.SetColor("_Color", Color.white);
        enabled = true;
        behaviour.enabled = true;
    }
}
