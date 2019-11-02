using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Shooter), typeof(NavMeshAgent))]
public class Cultist : Character
{
    private Shooter shooter;
    private NavMeshAgent agent;


    private void OnValidate()
    {

    }

    protected override void Awake()
    {
        base.Awake();
        shooter = GetComponent<Shooter>();
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start()
    {
        GameManager.Instance.ourCrew.Add(gameObject);

        GameManager.Instance.OnLeftButton.AddListener(GoToMousePosition);
        GameManager.Instance.OnRigthButton.AddListener(AimToMousePosition);
        GameManager.Instance.OnRigthButton.AddListener(shooter.StartShooting);
    }

    protected override void Update()
    {
        base.Update();

        (GameObject, float) aaa = GameManager.Instance.enemies.NearestFrom(transform.position);

        if ( aaa.Item2 > shooter.range )
        {
            shooter.StopShooting();
        }
    }

    public void GoToMousePosition()
    {
        

        int index = GameManager.Instance.ourCrew.IndexOf(gameObject);

        switch(index)
        {
            case 0:
                agent.destination = GameManager.Instance.MousePos;
                break;
            case 1:
                agent.destination = GameManager.Instance.MousePos + Vector2.down;
                break;
            case 2:
                agent.destination = GameManager.Instance.MousePos + Vector2.right;
                break;
            case 3:
                agent.destination = GameManager.Instance.MousePos + Vector2.right + Vector2.down;
                break;
            case 4:
                agent.destination = GameManager.Instance.MousePos + Vector2.up;
                break;
            case 5:
                agent.destination = GameManager.Instance.MousePos + Vector2.right + Vector2.up;
                break;
            case 6:
                agent.destination = GameManager.Instance.MousePos + Vector2.left;
                break;
            case 7:
                agent.destination = GameManager.Instance.MousePos + Vector2.left + Vector2.down;
                break;
            case 8:
                agent.destination = GameManager.Instance.MousePos + Vector2.left + Vector2.up;
                break;
            default:
                agent.destination = GameManager.Instance.MousePos;
                break;


        }

    }    
    public void AimToMousePosition() => GetComponent<Shooter>().target = GameManager.Instance.MousePos;
}
