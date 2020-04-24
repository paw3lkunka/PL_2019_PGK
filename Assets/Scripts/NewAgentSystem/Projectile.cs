using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damageMin;
    public float damageMax;
    public float range;
    public float speed;

    private Vector3 stepVector;
    private float stepScalar;
    private float distance;
    public void ShootTo(Vector3 target)
    {
        stepVector = (target - transform.position).normalized * speed;
        stepScalar = stepVector.magnitude;
        distance = 0;
    }

    #region MonoBehaviour

    private void FixedUpdate()
    {
        if(distance < range)
        {
            transform.position += stepVector;
            distance += stepScalar;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Damageable victim = collision.gameObject.GetComponent<Damageable>();

        if(victim)
        {
            victim.Damage(Random.Range(damageMin, damageMax));
        }

        Destroy(gameObject);
    }

    #endregion
}
