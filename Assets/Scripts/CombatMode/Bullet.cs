using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    public int damege;
    public float startSpeed;
    public float timeToDestroy;

    private new Collider2D collider;




    void Awake()
    {
        collider = GetComponent<Collider2D>();


    }

    public void Shoot( Vector2 direction )
    {
        IEnumerator routine()
        {
            GetComponent<Rigidbody2D>().AddForce(direction.normalized * startSpeed, ForceMode2D.Impulse);
            collider.enabled = false;

            yield return new WaitForSeconds(.1F);

            collider.enabled = true;
        }

        StartCoroutine(routine());
    }

    // Update is called once per frame
    void Update()
    {
        if( timeToDestroy < 0 )
        {
            Destroy(gameObject);
        }
        timeToDestroy -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }


}
