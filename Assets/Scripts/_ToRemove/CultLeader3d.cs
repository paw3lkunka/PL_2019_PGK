using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.InputSystem;

public class CultLeader3d : Character3d
{
    #region Variables

    [Header("Standard mode behaviour")]
    public float standardSpeed;
    public float standardDefence;

    [Header("Rage mode behaviour")]
    public float rageSpeed;
    public float rageDefence;

    private NewInput input;
    private bool canMove;

    #endregion

    #region MonoBehaviour

    protected override void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        SceneManager.sceneUnloaded += OnSceneUnload;
        ApplicationManager.Instance.OnGameOver += OnGameOver;

        DontDestroyOnLoad(gameObject);
        GameplayManager.Instance.ourCrew.Add(gameObject);

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "MainMap" || sceneName == "MainMenu")
        {
            gameObject.SetActive(false);
        }

        base.Awake();
        OnSceneLoad(SceneManager.GetActiveScene(),LoadSceneMode.Single);

        Agent.updateRotation = false;
        Agent.updateUpAxis = false;

        input = ApplicationManager.Instance.Input;
    }

    protected override void Start()
    {
        defence = standardDefence;
        Agent.speed = standardSpeed;

        try
        {
            RhythmMechanics.Instance.OnRageStart += EnterRageMode;
        }
        catch (System.NullReferenceException) { }

        try
        {
            RhythmMechanics.Instance.OnRageStop += ExitRageMode;
        }
        catch (System.NullReferenceException) { }

        hp = 100;
        defence = 20;
        base.Start();
    }

    protected void OnEnable()
    {
        if(input != null)
        {
            input.Gameplay.SetWalkTarget.performed += GoToCursorPosition;
        }

        if (CombatSceneManager.Instance.sceneMode == CombatSceneMode.Hostile)
        {
            gameObject.transform.GetComponentInChildren<HealthBar>().gameObject.SetActive(true);
        }
        else
        {
            // Hide HealthBar on neutral scenes
            gameObject.transform.GetComponentInChildren<HealthBar>().gameObject.SetActive(false);
        }
    }

    protected void OnDisable()
    {
        if (input != null)
        {
            input.Gameplay.SetWalkTarget.performed -= GoToCursorPosition;
        }
    }

    protected override void Update()
    {
        canMove = CheckState(CharacterState.CanMove);

        if (GameplayManager.Instance.ourCrew.Count == 1)
        {
            defence = 0;
        }

        base.Update();
    }

    private void OnDestroy()
    {
        try
        {
            RhythmMechanics.Instance.OnRageStart -= EnterRageMode;
        }
        catch (System.NullReferenceException) { }

        try
        {
            RhythmMechanics.Instance.OnRageStop -= ExitRageMode;
        }
        catch (System.NullReferenceException) { }

        SceneManager.sceneLoaded -= OnSceneLoad;
        SceneManager.sceneUnloaded -= OnSceneUnload;
    }

    #endregion

    #region Component

    private void EnterRageMode()
    {
        Agent.speed = rageSpeed;
        defence = rageDefence;
    }

    private void ExitRageMode()
    {
        Agent.speed = standardSpeed;
        defence = standardDefence;
    }

    public Vector3 FormationOffset
    {
        get
        {
            return Vector3.zero;
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
                Agent.Warp(CombatSceneManager.Instance.startPoint.position + FormationOffset);
                CombatSceneManager.Instance.cultLeaderTransform = transform;
                break;
        }
    }

    private void OnSceneUnload(Scene scene)
    {
        if (scene.name != "MainMap" && scene.name != "MainMenu")
        {
            GameplayManager.Instance.ourCrew.Remove(gameObject);
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        ApplicationManager.Instance.GameOver();
        ApplicationManager.Instance.OnGameOver -= OnGameOver;
        base.Die();
    }

    private void OnGameOver()
    {
        Destroy(gameObject);
        ApplicationManager.Instance.OnGameOver -= OnGameOver;
    }

    #endregion

    #region Input


    private void GoToCursorPosition(InputAction.CallbackContext ctx)
    {
        // TODO: Check gui mouse over
        //if (Agent.enabled && !ApplicationManager.Gui.isMouseOver && canMove)
        //{
            var cursorPosition = CombatCursorManager.Instance.mainCursor.transform.position;
            var nextDestination = cursorPosition + FormationOffset;

            Agent.SetDestination(nextDestination);
        //}
    }

    private void AimToCursorPosition(InputAction.CallbackContext ctx)
    {
        var cursorPosition = CombatCursorManager.Instance.mainCursor.transform.position;
        var nextTarget = cursorPosition + FormationOffset;

        GetComponent<Shooter3d>().target = nextTarget;
    }

    #endregion
}
