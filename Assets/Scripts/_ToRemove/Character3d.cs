using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[System.Flags]
public enum CharacterState
{
    CanMove = 0b_0000_0001,
    CanAttack = 0b_0000_0010
}

[RequireComponent(typeof(Collider), typeof(NavMeshAgent))]
public class Character3d : MonoBehaviour
{
    #region Variables

    // Unity Editor accesible fields
#pragma warning disable
    [SerializeField] protected int hp;
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
    public Rigidbody RBody { get; private set; }
    public TextsEmitter healthTextEmitter;
    public TextsEmitter faithTextEmitter;

    private int maxHp;
    private HealthBar healthBar;

    #endregion

    #region MonoBehaviour

    protected virtual void Awake()
    {
        SetStateOn(CharacterState.CanMove);
        SetStateOn(CharacterState.CanAttack);
        Agent = GetComponent<NavMeshAgent>();
        RBody = GetComponent<Rigidbody>();
    }

    protected virtual void Start()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        maxHp = hp;
    }

    protected virtual void Update()
    {
        if (hp <= 0)
        {
            Die();
        }

        healthBar?.SetBar(hp, maxHp);
    }

    #endregion

    #region Component

    public virtual void TakeDamage(int damage)
    {
        int realDamage = Mathf.RoundToInt((float)damage / (defence + 1));
        hp -= Mathf.Max(realDamage, 0);

        if (healthTextEmitter && realDamage > 0)
        {
            healthTextEmitter.Emit("-" + realDamage, Color.red, 2f);
        }
    }

    public virtual void Die()
    {
        GameplayManager.Instance.ourCrew.Remove(gameObject);
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

    #endregion
}
