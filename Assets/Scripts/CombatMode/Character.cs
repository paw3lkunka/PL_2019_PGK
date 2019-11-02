using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Character : MonoBehaviour
{
#pragma warning disable
    [SerializeField]
    private int hp;
    [SerializeField]
    [Range(0, 20)]
    private int defence;
#pragma warning restore
    
    private int maxHp;
    private HealthBar healthBar;

    protected virtual void Awake()
    {
        maxHp = hp;
    }

    protected virtual void Start()
    {
        healthBar = transform.Find("HealthBar")?.GetComponent<HealthBar>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(hp<=0)
        {
            Destroy(gameObject);
            GameManager.Instance.ourCrew.Remove(gameObject);
            GameManager.Instance.enemies.Remove(gameObject);
        }

        healthBar?.SetBar(hp, maxHp);
    }

    public void takeDamage(int damage)
    {
        hp -= Mathf.Max( damage / (defence+1) , 0);
    }


}
