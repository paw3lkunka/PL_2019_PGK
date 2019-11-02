using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Shooter),typeof(NavMeshAgent))]
public class Cultist : Character
{
    public float speed;
    private Shooter shooter;
    private NavMeshAgent agent;


    private void Awake()
    {
        shooter = GetComponent<Shooter>();
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start()
    {
        GameManager.Instance.OnLeftButton.AddListener( GoToMousePosition );
        GameManager.Instance.OnRigthButton.AddListener( AimToMousePosition );
        GameManager.Instance.OnRigthButton.AddListener( shooter.StartShooting );
    }

    private void Update()
    {
        if( Vector2.Distance( transform.position, GameManager.Instance.enemies.NearestFrom(transform.position)?.transform.position ?? Vector2.positiveInfinity ) > 10)
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
