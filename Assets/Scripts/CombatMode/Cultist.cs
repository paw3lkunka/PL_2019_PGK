using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(Shooter))]
public class Cultist : Character
{
    private Shooter shooter;
   
    protected override void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            gameObject.SetActive(false);
        }

        base.Awake();
        shooter = GetComponent<Shooter>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    protected override void Start()
    {
        base.Start();

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            CombatSceneManager.Instance.ourCrew.Add(gameObject);
            agent.Warp( CombatSceneManager.Instance.startPoint + FormationOffset );
        }

    }

    protected override void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        

    }

    protected override void OnSceneUnload(Scene scene)
    {
        if (scene.buildIndex == 0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    protected override void Update()
    {
        (GameObject, float) aaa = CombatSceneManager.Instance.enemies.NearestFrom(transform.position);

        if ( aaa.Item2 > shooter.range )
        {
            shooter.StopShooting();
        }

        if (Input.GetMouseButtonDown(0))
        {
            GoToMousePosition();
        }

        if (Input.GetMouseButtonDown(1))
        {
            AimToMousePosition();
            shooter.StartShooting();
        }

        base.Update();
    }

    public void GoToMousePosition()
    {
        agent.SetDestination(CombatSceneManager.Instance.MousePos + FormationOffset);
    }    
    public void AimToMousePosition() => GetComponent<Shooter>().target = CombatSceneManager.Instance.MousePos;

    public override void TakeDamage(int damage)
    {
        GameManager.Instance.Faith -= GameManager.Instance.FaithForWoundedCultist;
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        GameManager.Instance.Faith -= GameManager.Instance.FaithForKilledCultist;
        base.Die();
    }

    public Vector2 FormationOffset
    {
        get
        {
            int index = CombatSceneManager.Instance.ourCrew.IndexOf(gameObject);
            switch (index)
            {
                default:
                case 0:
                    return Vector2.zero;
                case 1:
                    return Vector2.down;
                case 2:
                    return Vector2.right;
                case 3:
                    return Vector2.right + Vector2.down;
                case 4:
                    return Vector2.up;
                case 5:
                    return Vector2.right + Vector2.up;
                case 6:
                    return Vector2.left;
                case 7:
                    return Vector2.left + Vector2.down;
                case 8:
                    return Vector2.left + Vector2.up;
            }
        }
    }
}
