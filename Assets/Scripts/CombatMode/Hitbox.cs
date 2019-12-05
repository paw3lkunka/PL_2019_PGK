using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hitbox : MonoBehaviour
{
    public Character character;
    bool stuned = false;

    private void Start()
    {
        if(!character)
        {
            character = GetComponentInParent<Character>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.GetComponentInChildren<Bullet>();
        /*
        IEnumerator Routine()
        {
            stuned = true;
            float timer = bullet.stunTime;
            Vector2 destination = character.Agent.destination;

            character.Agent.enabled = false;
            character.RBody2d.bodyType = RigidbodyType2D.Dynamic;

            character.GetComponent<Rigidbody2D>().AddForce(bullet.Direction * bullet.pushForce, ForceMode2D.Impulse);

            yield return new WaitForEndOfFrame();

            while ( timer > 0 )
            {
                timer -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            
            character.RBody2d.bodyType = RigidbodyType2D.Kinematic;
            character.Agent.enabled = true;
            character.Agent.SetDestination(destination);
            stuned = false;
        }*/

        if (bullet)
        {
            /*if(!stuned)
            {
                StartCoroutine(Routine());
            }*/
            TakeDamage(bullet.damege);
            Destroy(bullet.gameObject);
        }
    }

    public void TakeDamage(int damage) => character.TakeDamage(damage);


}
