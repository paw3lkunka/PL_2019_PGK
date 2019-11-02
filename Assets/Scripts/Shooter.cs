using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public bool enemy = true;
    public float baseDamage = 5;
    public float attackInterval = 1.5f;

    public GameObject bulletPrefab;
    public Vector2 target;

    // Start is called before the first frame update
    void Start()
    {
        if( enemy )
        {
            GameManager.Instance.enemies.Add(gameObject);
        }
        else
        {
            GameManager.Instance.ourCrew.Add(gameObject);
        }

        IEnumerator aaa()
        {
            while (true)
            {
                Debug.Log("Aaa");
                ShootTo(target);
                yield return new WaitForSeconds(attackInterval);
            }
        }

        StartCoroutine(aaa());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootTo( Vector2 target )
    {
        Instantiate(bulletPrefab,transform).GetComponent<Bullet>().Shoot( target - (Vector2)transform.position);
    }

    public GameObject Nearstemy
    {
        get
        {
            GameObject target = null;
            float distance = float.PositiveInfinity;
            foreach (GameObject enemy in Enemies)
            {
                if (Vector2.Distance(transform.position, enemy.transform.position) < distance)
                {
                    target = enemy;
                }
            }
            return target;
        }
    }

    public List<GameObject> Enemies
    {
        get
        {
            if (!enemy)
                return GameManager.Instance.enemies;
            else
                return GameManager.Instance.ourCrew;
        }
    }
}
