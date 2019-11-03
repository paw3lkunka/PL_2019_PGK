using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Shooter))]
public class Cultist : Character
{
    private Shooter shooter;


    private void OnValidate()
    {

    }

    protected override void Awake()
    {
        base.Awake();
        shooter = GetComponent<Shooter>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    protected override void Start()
    {
        base.Start();

        GameManager.Instance.ourCrew.Add(gameObject);

        GameManager.Instance.OnLeftButton.AddListener(GoToMousePosition);
        GameManager.Instance.OnRigthButton.AddListener(AimToMousePosition);
        GameManager.Instance.OnRigthButton.AddListener(shooter.StartShooting);

        transform.position = GameManager.Instance.startArea.transform.position + (Vector3)FormationOffset;
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
        agent.SetDestination(GameManager.Instance.MousePos + FormationOffset);
    }    
    public void AimToMousePosition() => GetComponent<Shooter>().target = GameManager.Instance.MousePos;

    public Vector2 FormationOffset
    {
        get
        {
            int index = GameManager.Instance.ourCrew.IndexOf(gameObject);
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
