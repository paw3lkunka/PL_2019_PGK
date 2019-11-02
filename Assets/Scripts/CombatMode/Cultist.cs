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

    private void Awake()
    {
        shooter = GetComponent<Shooter>();
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start()
    {
        GameManager.Instance.ourCrew.Add(gameObject);

        GameManager.Instance.OnLeftButton.AddListener( GoToMousePosition );
        GameManager.Instance.OnRigthButton.AddListener( AimToMousePosition );
        GameManager.Instance.OnRigthButton.AddListener( shooter.StartShooting );
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
        agent.destination = GameManager.Instance.MousePos;
    }
    public void AimToMousePosition()
    {
        GetComponent<Shooter>().target = GameManager.Instance.MousePos;
    }
}
