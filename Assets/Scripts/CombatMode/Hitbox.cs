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

        IEnumerator Routine()
        {
            stuned = true;
            float timer = bullet.stunTime;

            Vector2 vec = bullet.Direction * bullet.pushForce;
            Vector3 push = new Vector3( vec.x, vec.y, 0.0f );

            Transform spriteTransform = character.transform.GetChild(0).transform;
            spriteTransform.localPosition += push;
           

            yield return new WaitForEndOfFrame();

            while (timer > 0)
            {
                timer -= Time.deltaTime;
                spriteTransform.localPosition -= push * (Time.deltaTime / bullet.stunTime);
                yield return new WaitForEndOfFrame();
            }
            character.transform.GetChild(0).transform.localPosition = character.transform.GetComponentInChildren<HealthBar>(true).gameObject.transform.localPosition - new Vector3(0, 0.5f, 0);
            stuned = false;
        }

        if (bullet)
        {
            if (!stuned)
            {
                StartCoroutine(Routine());
            }
            TakeDamage(bullet.damege);
            Destroy(bullet.gameObject);
        }
    }

    public void TakeDamage(int damage) => character.TakeDamage(damage);


}
