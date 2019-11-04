using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(Shooter))]
public class Cultist : Character
{
    private Shooter shooter;

    private void OnValidate()
    {

    }

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

        SceneManager.sceneLoaded += OnSceneLoad;
        SceneManager.sceneUnloaded += OnSceneUnload;
    }

    protected override void Start()
    {
        base.Start();

    }

    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if( scene.buildIndex != 0 )
        {
            gameObject.SetActive(true);

            CombatSceneManager.Instance.ourCrew.Add(gameObject);

            CombatSceneManager.Instance.OnLeftButton.AddListener(GoToMousePosition);
            CombatSceneManager.Instance.OnRigthButton.AddListener(AimToMousePosition);
            CombatSceneManager.Instance.OnRigthButton.AddListener(shooter.StartShooting);

            agent.Warp(CombatSceneManager.Instance.startArea.transform.position + (Vector3)FormationOffset);
        }
    }

    public void OnSceneUnload(Scene scene)
    {
        gameObject.SetActive(false);
    }

    protected override void Update()
    {
        (GameObject, float) aaa = CombatSceneManager.Instance.enemies.NearestFrom(transform.position);

        if ( aaa.Item2 > shooter.range )
        {
            shooter.StopShooting();
        }

        base.Update();
    }

    public void GoToMousePosition()
    {
        agent.SetDestination(CombatSceneManager.Instance.MousePos + FormationOffset);
    }    
    public void AimToMousePosition() => GetComponent<Shooter>().target = CombatSceneManager.Instance.MousePos;

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
