using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine;

public class CultLeader : Character
{
    protected override void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        SceneManager.sceneUnloaded += OnSceneUnload;
        GameManager.Instance.OnGameOver += OnGameOver;

        DontDestroyOnLoad(gameObject);
        GameManager.Instance.ourCrew.Add(gameObject);

        string sceneName = SceneManager.GetActiveScene().name;
        if ( sceneName == "MainMap" || sceneName == "MainMenu")
        {
            gameObject.SetActive(false);
        }

        base.Awake();

        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        SceneManager.sceneUnloaded -= OnSceneUnload;
    }

    protected override void Start()
    {

    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMap" || scene.name == "MainMenu" )
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            Agent.Warp(CombatSceneManager.Instance.startPoint + FormationOffset);
        }
    }

    private void OnSceneUnload(Scene scene)
    {
        if (scene.name != "MainMap" && scene.name != "MainMenu")
        {
            GameManager.Instance.ourCrew.Remove(gameObject);
            CombatSceneManager.Instance.enemies.Remove(gameObject);
        }
    }

    protected override void Update()
    {
        bool canMove = CheckState(CharacterState.CanMove);

        if (canMove && Input.GetMouseButtonDown(0))
        {
            GoToMousePosition();
        }

        base.Update();
    }



    public void GoToMousePosition()
    {
        Agent.SetDestination(CombatSceneManager.Instance.MousePos + FormationOffset);
    }
    public void AimToMousePosition() => GetComponent<Shooter>().target = CombatSceneManager.Instance.MousePos + FormationOffset;

    public override void TakeDamage(int damage)
    {
        
    }

    public override void Die()
    {
        
    }

    private void OnGameOver()
    {
        Destroy(gameObject);
        GameManager.Instance.OnGameOver -= OnGameOver;
    }

    public Vector2 FormationOffset
    {
        get
        {
            return Vector2.zero;
        }
    }
}
