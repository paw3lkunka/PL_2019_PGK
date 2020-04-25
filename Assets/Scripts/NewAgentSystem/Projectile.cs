using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    public float damageMin = 1;
    public float damageMax = 1;
    public float range = 10;
    public float speed = 1;

    //TODO push force
    //Stun

    private Vector3 stepVector;
    private float stepScalar;
    private float distance;

    //TODO documentation
    public void ShootAt(Vector3 target)
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
            //todo layer mask
            victim.Damage(Random.Range(damageMin, damageMax));
        }

        Destroy(gameObject);
    }

    #endregion
}
