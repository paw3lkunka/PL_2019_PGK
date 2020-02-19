using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.InputSystem;

public class CultLeader : Character
{
    #region Variables

    private NewInput input;
    private bool canMove;

    #endregion

    #region MonoBehaviour

    protected override void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        SceneManager.sceneUnloaded += OnSceneUnload;
        GameManager.Instance.OnGameOver += OnGameOver;

        DontDestroyOnLoad(gameObject);
        GameManager.Instance.ourCrew.Add(gameObject);

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "MainMap" || sceneName == "MainMenu")
        {
            gameObject.SetActive(false);
        }

        base.Awake();

        Agent.updateRotation = false;
        Agent.updateUpAxis = false;

        input = GameManager.Instance.input;
    }

    protected override void Start()
    {
        hp = 100;
        defence = 20;
        base.Start();
    }

    protected void OnEnable()
    {
        if(input != null)
        {
            input.Gameplay.SetWalkTarget.performed += GoToCursorPosition;
            input.Gameplay.SetWalkTarget.Enable();
        }
    }

    protected void OnDisable()
    {
        if (input != null)
        {
            input.Gameplay.SetWalkTarget.performed -= GoToCursorPosition;
            input.Gameplay.SetWalkTarget.Disable();
        }
    }

    protected override void Update()
    {
        canMove = CheckState(CharacterState.CanMove);

        if (GameManager.Instance.cultistNumber == 1)
        {
            defence = 0;
        }

        base.Update();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        SceneManager.sceneUnloaded -= OnSceneUnload;
    }

    #endregion

    #region Component

    public Vector2 FormationOffset
    {
        get
        {
            return Vector2.zero;
        }
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                gameObject.SetActive(false);
                break;

            case "MainMap":
                gameObject.SetActive(false);
                break;

            default:
                gameObject.SetActive(true);
                Agent.Warp(CrewSceneManager.Instance.startPoint + FormationOffset);
                CrewSceneManager.Instance.cultLeader = transform;
                break;
        }
    }

    private void OnSceneUnload(Scene scene)
    {
        if (scene.name != "MainMap" && scene.name != "MainMenu")
        {
            GameManager.Instance.ourCrew.Remove(gameObject);
            CrewSceneManager.Instance.enemies.Remove(gameObject);
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        GameManager.Instance.OnGameOver -= OnGameOver;
        base.Die();
    }

    private void OnGameOver()
    {
        Destroy(gameObject);
        GameManager.Instance.OnGameOver -= OnGameOver;
    }

    #endregion

    #region Input


    private void GoToCursorPosition(InputAction.CallbackContext ctx)
    {
        if (Agent.enabled && !GameManager.Gui.isMouseOver && canMove)
        {
            var cursorPosition = CrewSceneManager.Instance.cursorInstance.position;
            var nextDestination = new Vector2(cursorPosition.x, cursorPosition.y) + FormationOffset;

            Agent.SetDestination(nextDestination);
        }
    }

    private void AimToCursorPosition(InputAction.CallbackContext ctx)
    {
        var cursorPosition = CrewSceneManager.Instance.cursorInstance.position;
        var nextTarget = new Vector2(cursorPosition.x, cursorPosition.y) + FormationOffset;

        GetComponent<Shooter>().target = nextTarget;
    }

    #endregion
}
