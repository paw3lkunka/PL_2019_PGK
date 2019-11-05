using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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

        SceneManager.sceneLoaded += OnSceneLoad;
        SceneManager.sceneUnloaded += OnSceneUnload;
    }

    protected virtual void OnSceneUnload(Scene scene) { }
    protected virtual void OnSceneLoad(Scene scene, LoadSceneMode mode) { }

    protected virtual void Start()
    {
        healthBar = transform.Find("HealthBar")?.GetComponent<HealthBar>();
    }

    protected virtual void Update()
    {
        if (hp <= 0)
        {
            Die();
        }

        healthBar?.SetBar(hp, maxHp);
    }

    public void TakeDamage(int damage)
    {
        hp -= Mathf.Max( damage / (defence+1) , 0);
    }

    public void Die()
    {
        CombatSceneManager.Instance.ourCrew.Remove(gameObject);
        CombatSceneManager.Instance.enemies.Remove(gameObject);
        StartCoroutine(Routine());
        Debug.Log("Dead");

        IEnumerator Routine()
        {
            yield return new WaitForEndOfFrame();
            Destroy(gameObject);
        }
    }

}
