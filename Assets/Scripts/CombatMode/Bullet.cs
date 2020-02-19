using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    #region Variables

    public int damege;
    public float startSpeed;
    public float timeToDestroy;

    public float pushForce;
    public float stunTime;

    [field: SerializeField]
    public Vector2 Direction { get; private set; }

    private new Collider2D collider;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (timeToDestroy < 0)
        {
            Destroy(gameObject);
        }
        timeToDestroy -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    #endregion

    #region Component

    public void Shoot(Vector2 direction)
    {
        Direction = direction.normalized;

        IEnumerator routine()
        {
            GetComponent<Rigidbody2D>().AddForce(Direction * startSpeed, ForceMode2D.Impulse);
            collider.enabled = false;

            yield return new WaitForSeconds(.1F);

            collider.enabled = true;
        }

        StartCoroutine(routine());
    }

    #endregion
}
