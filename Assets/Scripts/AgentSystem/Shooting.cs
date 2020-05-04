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

    [field: Header("Normal")]

    [field: SerializeField, GUIName("DamageMultiplier")]
    public float DamageMultiplier { get; set; } = 1;

    [field: SerializeField, GUIName("RangeMultiplier")]
    public float RangeMultiplier { get; set; } = 1;

    [field: SerializeField, GUIName("IntervalMultiplier")]
    public float IntervalMultiplier { get; set; } = 1;

    public void Attack(Vector3 target)
    {
        this.target = target;

        if( (flags & Flags.canShoot) != 0 && shootRoutine == null )
        {
            shootRoutine = StartCoroutine(ShootRoutine());
        }
    }

    public void HoldFire()
    {
        if(shootRoutine != null)
        {
            StopCoroutine(shootRoutine);
            shootRoutine = null;
        }        
    }

    protected virtual Projectile CreateProjectile()
    {
        var projectile = Instantiate(ApplicationManager.prefabDatabase.projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
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

    private IEnumerator ShootRoutine()
    {
        while( (flags & Flags.canShoot) != 0 )
        {
            //TODO sync with rythm
            CreateProjectile().ShootAt(target);
            yield return new WaitForSecondsRealtime(IntervalMultiplier);
        }
        shootRoutine = null;
    }

    private Vector3 target;
    private Coroutine shootRoutine = null;

    #region MonoBehaviour

    private void OnValidate()
    {
        if(gameObject.layer != LayerMask.NameToLayer("PlayerCrew")
            && gameObject.layer != LayerMask.NameToLayer("Enemies"))
        {
            Debug.LogError("GameObject has invalid layer!");
        }
    }

    private void FixedUpdate()
    {
        if( (flags & Flags.canShoot) == 0 )
        {
            HoldFire();
        }
    }

    #endregion
}
