using System.Collections;
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
    [SerializeField] protected float defence;
    [Header("Read Only")]
    [SerializeField] private CharacterState characterState;
#pragma warning restore

    // Public field accesors
    public CharacterState CharacterState
    {
        get => characterState;
        set => characterState = value;
    }

    public NavMeshAgent Agent { get; private set; }
    public Rigidbody2D RBody2d { get; private set; }
    public TextsEmitter healthTextEmitter;
    public TextsEmitter fatihTextEemitter;

    private int maxHp;
    private HealthBar healthBar;

    protected virtual void Awake()
    {
        SetStateOn(CharacterState.CanMove);
        SetStateOn(CharacterState.CanAttack);
        maxHp = hp;
        Agent = GetComponent<NavMeshAgent>();
        RBody2d = GetComponent<Rigidbody2D>();
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
        int realDamage = Mathf.CeilToInt((float)damage / (defence + 1));
        hp -= Mathf.Max(realDamage, 0);

        if(emitter)
        {
            emitter.Emit("-" + realDamage, Color.red, 2f);
        }
    }

    public virtual void Die()
    {
        GameManager.Instance.ourCrew.Remove(gameObject);
        CombatSceneManager.Instance.enemies.Remove(gameObject);
        StartCoroutine(Routine());

        IEnumerator Routine()
        {
            yield return new WaitForEndOfFrame();
            Destroy(gameObject);
        }
    }

    public void FlipState(CharacterState state)
    {
        characterState ^= state;
    }

    public bool CheckState(CharacterState state)
    {
        return (characterState & state) > 0 ? true : false;
    }

    public void SetStateOn(CharacterState state)
    {
        characterState |= state;
    }

    public void SetStateOff(CharacterState state)
    {
        characterState &= ~state;
    }
}
