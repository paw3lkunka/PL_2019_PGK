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
        GameManager.Instance.OnGameOver += OnGameOver;
        DontDestroyOnLoad(gameObject);
        GameManager.Instance.ourCrew.Add(gameObject);

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "MainMap" || sceneName == "MainMenu")
        {
            gameObject.SetActive(false);
        }

        base.Awake();
        OnSceneLoad(SceneManager.GetActiveScene(),LoadSceneMode.Single);

        Agent.updateRotation = false;
        Agent.updateUpAxis = false;

        input = GameManager.Instance.input;
    }

    protected override void Start()
    {
        Agent.Warp(CrewSceneManager3d.Instance.startPoint + FormationOffset);
        //CrewSceneManager3d.Instance.cultLeader = transform; // ! Moved as a Crew Scene Manager responsibility

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
        
        if (input != null)
        {
            input.Gameplay.SetWalkTarget.performed += GoToCursorPosition;
        }

        if (FindObjectOfType<CrewSceneManager3d>().combatMode)
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

        if (GameManager.Instance.cultistNumber == 1 || GameManager.Instance.ourCrew.Count == 1)
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
                break;
        }
    }

    private void OnSceneUnload(Scene scene)
    {
        if (scene.name != "MainMap" && scene.name != "MainMenu")
        {
            GameManager.Instance.ourCrew.Remove(gameObject);
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        GameManager.Instance.GameOver();
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
            var cursorPosition = CrewSceneManager3d.Instance.cursorInstance.position;
            var nextDestination = cursorPosition + FormationOffset;

            Agent.SetDestination(nextDestination);
        }
    }

    private void AimToCursorPosition(InputAction.CallbackContext ctx)
    {
        var cursorPosition = CrewSceneManager3d.Instance.cursorInstance.position;
        var nextTarget = cursorPosition + FormationOffset;

        GetComponent<Shooter>().target = nextTarget;
    }

    #endregion
}
