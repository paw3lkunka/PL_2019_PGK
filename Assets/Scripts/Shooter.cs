using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class Shooter : MonoBehaviour
{
    public float range = 10;
    public float baseDamage = 5;
    public float attackInterval = 1.5f;

    public GameObject bulletPrefab;
    public Vector2 target;

    private Coroutine shooting;    

    public void ShootTo( Vector2 target )
    {
        GameObject obj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        if (gameObject.layer == LayerMask.NameToLayer("PlayerCrew"))
            obj.layer = LayerMask.NameToLayer("PlayerBullets");

        if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
            obj.layer = LayerMask.NameToLayer("EnemiesBullets");

        obj.GetComponent<Bullet>().Shoot( target - (Vector2)transform.position);
    }

    private IEnumerator ShootingRoutine()
    {
        while (true)
        {
            ShootTo(target);

            yield return new WaitForSeconds(attackInterval);
        }
    }

    public void StartShooting()
    {
        Debug.Log("Start");
        if( shooting == null)
        {
            shooting = StartCoroutine(ShootingRoutine());
        }
    }
    public void StopShooting()
    {
        Debug.Log("Stop");
        if (shooting != null)
        {

            Debug.Log("Real");
            StopCoroutine(shooting);
        }
        shooting = null;
    }
}
