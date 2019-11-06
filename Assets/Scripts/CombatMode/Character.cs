﻿using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[System.Flags]
public enum CharacterState
{
    CanMove     = 0b_0000_0001,
    CanAttack   = 0b_0000_0010
}

[RequireComponent(typeof(Collider2D), typeof(NavMeshAgent))]
public class Character : MonoBehaviour
{
    // Unity Editor accesible fields
#pragma warning disable
    [SerializeField] private int hp;
    [Range(0, 20)]
    [SerializeField] private int defence;
    [Header("Read Only")]
    [SerializeField] private CharacterState characterState;
#pragma warning restore

    // Public field accesors
    public CharacterState CharacterState
    {
        get => characterState;
        set => characterState = value;
    }

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
        healthBar = GetComponentInChildren<HealthBar>();
    }

    protected virtual void Update()
    {
        if (hp <= 0)
        {
            Die();
        }

        healthBar?.SetBar(hp, maxHp);
    }

    public virtual void TakeDamage(int damage)
    {
        hp -= Mathf.Max( damage / (defence + 1) , 0);
    }

    public virtual void Die()
    {
        CombatSceneManager.Instance.ourCrew.Remove(gameObject);
        CombatSceneManager.Instance.enemies.Remove(gameObject);
        StartCoroutine(Routine());

        IEnumerator Routine()
        {
            yield return new WaitForEndOfFrame();
            Destroy(gameObject);
        }
    }
}
