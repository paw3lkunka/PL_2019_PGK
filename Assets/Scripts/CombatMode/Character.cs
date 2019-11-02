using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D),typeof(SpriteRenderer))]
public class Character : MonoBehaviour
{
    [SerializeField]
    private int hp;
    [SerializeField]
    [Range(0, 20)]
    private int defence;
#pragma warning restore
    
    private int maxHp;
    private SpriteRenderer renderer;
    private HealthBar healthBar;

    protected virtual void Awake()
    {
        maxHp = hp;
    }

    protected virtual void Start()
    {
        healthBar = transform.Find("HealthBar")?.GetComponent<HealthBar>();
        renderer = GetComponent<SpriteRenderer>();
    }
        healthBar?.SetBar(hp, maxHp);
    }

    public void takeDamage(int damage)
    {
        hp -= Mathf.Max( damage / (defence+1) , 0);
    }


}
