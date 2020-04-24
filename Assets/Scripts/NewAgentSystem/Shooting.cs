using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public enum Flags
    {
        canShoot = 0b0000_0001,
    }

    public GameObject projectilePrefab;

    public Flags flags;


    [field: SerializeField, GUIName("DamageMultiplier")]
    public float DamageMultiplier { get; set; }

    [field: SerializeField, GUIName("RangeMultiplier")]
    public float RangeMultiplier { get; set; }

    [field: SerializeField, GUIName("IntervalMultiplier")]
    public float IntervalMultiplier { get; set; }

    public void Shoot(Vector3 target)
    {
        this.target = target;

        if( (flags & Flags.canShoot) != 0 && shootRoutine == null )
        {
            shootRoutine = StartCoroutine(ShootRoutine());
        }
    }

    protected virtual Projectile CreateProjectile()
    {
        var projectile = Instantiate(projectilePrefab).GetComponent<Projectile>();
        projectile.damageMin *= DamageMultiplier;
        projectile.damageMax *= DamageMultiplier;
        projectile.range *= RangeMultiplier;
        return projectile;
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
        if (!projectilePrefab.GetComponent<Projectile>())
        {
            Debug.LogError("ProjectilePrefab is invalid!");
        }
    }

    private void FixedUpdate()
    {
        if( (flags & Flags.canShoot) == 0 )
        {
            StopCoroutine(shootRoutine);
            shootRoutine = null;
        }
    }

    #endregion
}
