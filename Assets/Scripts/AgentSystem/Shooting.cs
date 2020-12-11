using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour, IAttack
{
    public enum Flags
    {
        Nothing = 0b0000_0000,
        canShoot = 0b0000_0001,
    }

    public Flags flags = Flags.canShoot;

    public AudioSource audioSource;
    public AudioClip shootingSound;

//TODO: do it better
    [field: Header("If synchronived with rythm, interval value means")]
    [field: Header("amount of shoots per tact, and should be power of 2.")]
    [field: Header("Otherwise interval value means time in seconds.")]
    [field: Header("== Dont change SyncWithRhythm in play mode! ==")]
    [field: Space]

    [field: SerializeField, GUIName("SyncWithRhythm")]
    public bool SyncWithRhythm { get; private set; }

    [field: Header("Normal")]

    [field: SerializeField, GUIName("DamageMultiplier")]
    public float DamageMultiplier { get; set; } = 1;

    [field: SerializeField, GUIName("RangeMultiplier")]
    public float RangeMultiplier { get; set; } = 1;

    [field: SerializeField, GUIName("IntervalBase")]
    public float IntervalBase { get; set; } = 1;


    public virtual float Interval { get => IntervalBase; }

    public int IntervalInt { get => Mathf.RoundToInt(Interval); }

    private Vector3 shootTarget;
    private bool AttackingInRythm { get; set; }
    private bool AttackingOutRythm { get => attackingOutOfRythmCoroutine != null; }
    private Coroutine attackingOutOfRythmCoroutine = null;


    public void Attack(Vector3 target)
    {
        shootTarget = target;

        if ((flags & Flags.canShoot) != 0)
        {
            if (SyncWithRhythm && !AttackingInRythm)
            {
                StartShootingInRhythm();
            }
            else if (!AttackingOutRythm)
            {
                StartShootingOutOfRhythm();
            }
        }
    }

    public void HoldFire()
    {
        if (AttackingInRythm)
        {
            StopShootingInRhythm();
        }
        else if(AttackingOutRythm)
        {
            StopShootingOutOfRhythm();
        }        
    }

    protected virtual Projectile CreateProjectile()
    {
        var projectile = Instantiate(ApplicationManager.Instance.PrefabDatabase.projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        projectile.damageMin *= DamageMultiplier;
        projectile.damageMax *= DamageMultiplier;
        projectile.range *= RangeMultiplier;
        SetLayerMask(projectile);
        return projectile;
    }

    protected void SetLayerMask(Projectile proj)
    {
        var obj = proj.gameObject;

        if (gameObject.layer == LayerMask.NameToLayer("PlayerCrew"))
        {
            obj.layer = LayerMask.NameToLayer("PlayerBullets");
        }
        else if (gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            obj.layer = LayerMask.NameToLayer("EnemiesBullets");
        }
        else
        {
            Debug.LogError($"GameObject {gameObject.name} has invalid layer!");
        }
    }

    private void StartShootingInRhythm()
    {
        AttackingInRythm = true;
        AudioTimeline.Instance.OnSubdiv += ShootInRythm;
    }

    private void StopShootingInRhythm()
    {
        AttackingInRythm = false;
        AudioTimeline.Instance.OnSubdiv -= ShootInRythm;
    }

    private void StartShootingOutOfRhythm()
    {
        attackingOutOfRythmCoroutine = StartCoroutine(ShootOutOfRhythmRoutine());
    }

    private void StopShootingOutOfRhythm()
    {
        StopCoroutine(attackingOutOfRythmCoroutine);
        attackingOutOfRythmCoroutine = null;
    }

    private  void ShootInRythm(int subdiv)
    {
        if (subdiv <= IntervalInt)
        {
        //Debug.Log($"O: {name} - shoot on subdiv = {subdiv}");
            Shoot();
        }
    }

    private IEnumerator ShootOutOfRhythmRoutine()
    {
        while( (flags & Flags.canShoot) != 0 )
        {
            //Debug.Log($"O: {name} - out of rhythm");
            Shoot();
            yield return new WaitForSecondsRealtime(Interval);
        }
        attackingOutOfRythmCoroutine = null;
    }

    void Shoot()
    {
        if (audioSource && shootingSound)
            audioSource.PlayOneShot(shootingSound);
        CreateProjectile().ShootAt(this, shootTarget);
    }


    #region MonoBehaviour

    private void OnValidate()
    {
        if (gameObject.layer != LayerMask.NameToLayer("PlayerCrew")
         && gameObject.layer != LayerMask.NameToLayer("Enemies"))
        {
            Debug.LogError("GameObject has invalid layer!");
        }

        if (SyncWithRhythm && !Mathf.IsPowerOfTwo(IntervalInt))
        {
            Debug.LogError("If SyncWithRhythm Interval mus be power of 2!");
        }
    }

    private void Awake()
    {
        AttackingInRythm = false;
        attackingOutOfRythmCoroutine = null;
    }

    private void FixedUpdate()
    {
        if( (flags & Flags.canShoot) == 0 )
        {
            HoldFire();
        }
    }

    private void OnDisable()
    {
        if(AudioTimeline.Instance != null)
        {
            AudioTimeline.Instance.OnSubdiv -= ShootInRythm;
        }
        StopAllCoroutines();
    }

    #endregion
}
