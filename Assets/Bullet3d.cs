using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Bullet3d : MonoBehaviour
{
    #region Variables

    public int damege;
    public float startSpeed;
    public float timeToDestroy;

    public float pushForce;
    public float stunTime;

    [field: SerializeField]
    public Vector3 Direction { get; private set; }

    private new Collider collider;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (timeToDestroy < 0)
        {
            Destroy(gameObject);
        }
        timeToDestroy -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    #endregion

    #region Component

    public void Shoot(Vector3 direction)
    {
        direction.y = 0.0f;
        Direction = direction.normalized;

        IEnumerator routine()
        {
            GetComponent<Rigidbody>().AddForce(Direction * startSpeed, ForceMode.Impulse);
            collider.enabled = false;

            yield return new WaitForSeconds(.1F);

            collider.enabled = true;
        }

        StartCoroutine(routine());
    }

    #endregion
}
