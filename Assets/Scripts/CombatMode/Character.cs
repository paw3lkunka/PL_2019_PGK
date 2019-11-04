using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider2D), typeof(NavMeshAgent))]
public class Character : MonoBehaviour
{
    [SerializeField]
    private int hp;
    [SerializeField]
    [Range(0, 20)]
    private int defence;
#pragma warning restore

    protected NavMeshAgent agent;

    private int maxHp;
    private HealthBar healthBar;


    protected virtual void Awake()
    {
        maxHp = hp;
        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        healthBar = transform.Find("HealthBar")?.GetComponent<HealthBar>();
    }

    protected virtual void Update()
    {
        if (hp <= 0)
        {
            Destroy(gameObject);
            CombatSceneManager.Instance.ourCrew.Remove(gameObject);
            CombatSceneManager.Instance.enemies.Remove(gameObject);
        }

        healthBar?.SetBar(hp, maxHp);
    }

    public void takeDamage(int damage)
    {
        hp -= Mathf.Max( damage / (defence+1) , 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();

        if (bullet)
        {
            takeDamage(bullet.damege);
            Destroy(bullet.gameObject);
        }
    }

}
