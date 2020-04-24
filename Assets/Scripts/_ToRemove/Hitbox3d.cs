using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hitbox3d : MonoBehaviour
{
    #region Variables

    public Character3d character;
    bool stuned = false;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        if (!character)
        {
            character = GetComponentInParent<Character3d>();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        var bullet = collision.GetComponentInChildren<Bullet3d>();

        IEnumerator Routine()
        {
            stuned = true;
            float timer = bullet.stunTime;

            Vector3 vec = bullet.Direction * bullet.pushForce;
            Vector3 push = new Vector3(vec.x, vec.y, 0.0f);

            Transform spriteTransform = character.transform.GetChild(0).transform;
            Vector3 backup = spriteTransform.localPosition;
            spriteTransform.localPosition += push;


            yield return new WaitForEndOfFrame();

            while (timer > 0)
            {
                timer -= Time.deltaTime;
                spriteTransform.localPosition -= push * (Time.deltaTime / bullet.stunTime);
                yield return new WaitForEndOfFrame();
            }
            character.transform.GetChild(0).transform.localPosition = backup;
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

    #endregion

    #region Component

    public void TakeDamage(int damage)
    {
        character.TakeDamage(damage);
    }

    #endregion
}
