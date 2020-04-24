using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character3d))]
public class Shooter3d : MonoBehaviour
{
    #region Variables

    public float range = 10;
    public float baseDamage = 5;
    public float attackInterval = 1.5f;
    public float variation = .1f;

    public GameObject bulletPrefab;
    public Vector3 target;

    private Coroutine shooting;

    #endregion

    #region MonoBehaviour



    #endregion

    #region Component

    public void ShootTo(Vector3 target)
    {
        GameObject obj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        if (gameObject.layer == LayerMask.NameToLayer("PlayerCrew"))
            obj.layer = LayerMask.NameToLayer("PlayerBullets");

        if (gameObject.layer == LayerMask.NameToLayer("Enemies"))
            obj.layer = LayerMask.NameToLayer("EnemiesBullets");

        obj.GetComponent<Bullet3d>().Shoot(target - transform.position);
    }

    private IEnumerator ShootingRoutine()
    {
        //needs fix
        //yield return new WaitForSeconds(Mathf.Clamp(Random.Range(-variation / 2, variation / 2), 0, variation / 2));

        while (true)
        {
            ShootTo(target);

            yield return new WaitForSeconds(attackInterval + Random.Range(-variation / 2, variation / 2));
        }
    }

    public void StartShooting()
    {
        if (shooting == null)
        {
            shooting = StartCoroutine(ShootingRoutine());
        }
    }

    public void StopShooting()
    {
        if (shooting != null)
        {
            StopCoroutine(shooting);
        }
        shooting = null;
    }

    #endregion
}
