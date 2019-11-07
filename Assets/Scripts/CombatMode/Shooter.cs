using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class Shooter : MonoBehaviour
{
    public float range = 10;
    public float baseDamage = 5;
    public float attackInterval = 1.5f;
    public float variation = .1f;

    public GameObject bulletPrefab;
    public Vector2 target;

    private Coroutine shooting;    

    public void ShootTo( Vector2 target )
    {
        GameObject obj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        if (gameObject.layer == LayerMask.NameToLayer("PlayerCrew"))
            obj.layer = LayerMask.NameToLayer("PlayerBullets");

        if (gameObject.layer == LayerMask.NameToLayer("Enemies"))
            obj.layer = LayerMask.NameToLayer("EnemiesBullets");

        obj.GetComponent<Bullet>().Shoot( target - (Vector2)transform.position);
    }

    private IEnumerator ShootingRoutine()
    {
        //needs fix
        //yield return new WaitForSeconds(Mathf.Clamp(Random.Range(-variation / 2, variation / 2), 0, variation / 2));

        while (true)
        {
            ShootTo(target);

            yield return new WaitForSeconds(attackInterval + Random.Range(-variation/2,variation/2));
        }
    }

    public void StartShooting()
    {
        if( shooting == null)
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

}
