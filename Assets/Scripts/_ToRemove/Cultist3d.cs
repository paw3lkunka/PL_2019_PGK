﻿using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Shooter3d))]
public class Cultist3d : Character3d
{
    #region Variables
    [Header("Standard mode behaviour")]
    public float standardDamage;
    public float standardSpeed;
    public float standardDefence;

    [Header("Rage mode behaviour")]
    public float rageDamage;
    public float rageSpeed;
    public float rageDefence;

    [Header("Fanatic behaviour")]
    [Tooltip("May perform fanatic behaviour")]
    public bool isFanatic;
    [Tooltip("Actully performs fanatic behaviour")]
    public bool fanaticState;
    [Range(0, 0.001f)]
    public float fanaticStateEnterChance = 0.05f;
    [Range(0, 0.001f)]
    public float fanaticStateExitChance = 0.01f;
    public float fanaticAimDebuff;


    private Shooter3d shooter;
    private NewInput input;
    
    private bool canMove;
    private bool canAttack;

    #endregion

    #region MonoBehaviour

    protected override void Awake()
    {
        defence = standardDefence;

        SceneManager.sceneLoaded += OnSceneLoad;
        SceneManager.sceneUnloaded += OnSceneUnload;
        ApplicationManager.Instance.OnGameOver += OnGameOver;

        DontDestroyOnLoad(gameObject);
        GameplayManager.Instance.ourCrew.Add(gameObject);
        gameObject.transform.position = GameplayManager.Instance.ourCrew[0].transform.position + new Vector3(FormationOffset.x, FormationOffset.y);

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "MainMap" || sceneName == "MainMenu")
        {
            gameObject.SetActive(false);
        }

        base.Awake();
        shooter = GetComponent<Shooter3d>();

        Agent.updateRotation = false;
        Agent.updateUpAxis = false;

        input = ApplicationManager.Instance.Input;
    }

    protected override void Start()
    {
        base.Start();

        shooter.baseDamage = standardDamage;
        Agent.speed = standardSpeed;

        isFanatic = GameplayManager.Instance.Faith > GameplayManager.Instance.fanaticFaithLevel;
        fanaticState = false;

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

        GameplayManager.Instance.FanaticStart += EnterFanaticMode;
        GameplayManager.Instance.FanaticEnd += ExitFanaticMode;

    }

    protected void OnEnable()
    {
        if(input != null)
        {
            input.Gameplay.SetWalkTarget.performed += GoToCursorPosition;
            input.CombatMode.SetShootTarget.performed += AimToCursorPosition;
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
        if(input != null)
        {
            input.Gameplay.SetWalkTarget.performed -= GoToCursorPosition;
            input.CombatMode.SetShootTarget.performed -= AimToCursorPosition;
        }
    }

    protected override void Update()
    {
        if (isFanatic)
        {
            if (fanaticState)
            {
                fanaticState = Random.Range(0.0f, 1.0f) < fanaticStateExitChance ? false : true;
                Debug.Log("enter");
            }
            else
            {
                Debug.Log("exit");
                fanaticState = Random.Range(0.0f, 1.0f) < fanaticStateEnterChance ? true : false;
            }
        }


        if (CombatSceneManager.Instance.sceneMode == CombatSceneMode.Hostile)
        {
            if (RhythmMechanics.Instance.Combo >= 1)
            {
                SetStateOn(CharacterState.CanAttack);
            }
            else
            {
                SetStateOff(CharacterState.CanAttack);
            }
        }

        (GameObject, float) nearest = CombatSceneManager.Instance.enemies.NearestFrom3d(transform.position);
        canMove = CheckState(CharacterState.CanMove);
        canAttack = CombatSceneManager.Instance.sceneMode == CombatSceneMode.Hostile && CheckState(CharacterState.CanAttack);

        if (fanaticState)
        {
            FanaticBehaviour(nearest.Item1, nearest.Item2, canMove, canAttack);
        }
        else
        {
            StandardBehaviour(nearest.Item1, nearest.Item2, canMove, canAttack);
        }

        base.Update();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        SceneManager.sceneUnloaded -= OnSceneUnload;

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

        GameplayManager.Instance.FanaticStart -= EnterFanaticMode;
        GameplayManager.Instance.FanaticEnd -= ExitFanaticMode;
    }

    #endregion

    #region Component

    #region Behaviour

    public void StandardBehaviour(GameObject enemy, float distanceToEnemy, bool canMove, bool canAttack)
    {
        if (!canAttack || distanceToEnemy > shooter.range)
        {
            shooter.StopShooting();
        }
    }

    public void FanaticBehaviour(GameObject enemy, float distanceToEnemy, bool canMove, bool canAttack)
    {
        if (canAttack && distanceToEnemy <= shooter.range)
        {
            Vector3 target = enemy.transform.position;

            if (canMove)
            {
                Agent.SetDestination(target);
            }

            shooter.target = target + new Vector3
            (
                Random.Range(-fanaticAimDebuff, fanaticAimDebuff),
                Random.Range(-fanaticAimDebuff, fanaticAimDebuff),
                0.0f
            );
            shooter.StartShooting();
        }
        else
        {
            shooter.StopShooting();
        }
    }

    #endregion

    #region Event Listeners

    private void EnterRageMode()
    {
        shooter.baseDamage = rageDamage;
        Agent.speed = rageSpeed;
        defence = rageDefence;
    }

    private void ExitRageMode()
    {
        shooter.baseDamage = standardDamage;
        Agent.speed = standardSpeed;
        defence = standardDefence;
    }

    private void EnterFanaticMode() => isFanatic = true;
    private void ExitFanaticMode() => isFanatic = false;

    /// <summary>
    /// SceneLoad listener
    /// </summary>
    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMap":
                gameObject.SetActive(false);
                break;

            case "MainMenu":
                gameObject.SetActive(false);
                break;

            default:
                gameObject.SetActive(true);
                Agent.Warp(CombatSceneManager.Instance.startPoint.position + FormationOffset);
                break;
        }
    }

    /// <summary>
    /// SceneUnload listener
    /// </summary>
    private void OnSceneUnload(Scene scene)
    {
        if (scene.name != "MainMap" && scene.name != "MainMenu")
        {
            CombatSceneManager.Instance.enemies.Remove(gameObject);
        }
    }

    /// <summary>
    /// GameOverEvent listener
    /// </summary>
    private void OnGameOver()
    {
        ApplicationManager.Instance.OnGameOver -= OnGameOver;
    }

    #endregion

    #region Taking damage

    public override void TakeDamage(int damage)
    {
        GameplayManager.Instance.Faith -= GameplayManager.Instance.faithForWoundedCultist;
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        ApplicationManager.Instance.OnGameOver -= OnGameOver;
        base.Die();

        float lostFaith = GameplayManager.Instance.faithForKilledCultist;
        GameplayManager.Instance.Faith -= lostFaith;
        faithTextEmitter.Emit("-" + (int)(lostFaith * 100), Color.green, 3);
    }

    #endregion
    
    #region misc
    /// <summary>
    /// Returns position in formation from formation centre
    /// </summary>
    public Vector3 FormationOffset
    {
        get
        {
            int index = GameplayManager.Instance.ourCrew.IndexOf(gameObject);
            switch (index % 8 + 1)
            {
                default:
                case 1:
                    return Vector3.back;
                case 2:
                    return Vector3.right;
                case 3:
                    return Vector3.right + Vector3.back;
                case 4:
                    return Vector3.forward;
                case 5:
                    return Vector3.right + Vector3.forward;
                case 6:
                    return Vector3.left;
                case 7:
                    return Vector3.left + Vector3.back;
                case 8:
                    return Vector3.left + Vector3.forward;
            }
        }
    }

    #endregion

    #endregion

    #region Input

    private void GoToCursorPosition(InputAction.CallbackContext ctx)
    {
        // TODO: Check for gui mouse over
        if (Agent.enabled/* && !ApplicationManager.Gui.isMouseOver*/ && canMove)
        {
            var cursorPosition = CombatCursorManager.Instance.MainCursor.transform.position;
            var nextDestination = cursorPosition + FormationOffset;
            Agent.SetDestination(nextDestination);
        }
    }

    private void AimToCursorPosition(InputAction.CallbackContext ctx)
    {
        if(canAttack)
        {
            var targetOffset = new Vector3
            (
                Random.Range(Mathf.Min(0, FormationOffset.x), Mathf.Max(0, FormationOffset.x)),
                Random.Range(Mathf.Min(0, FormationOffset.y), Mathf.Max(0, FormationOffset.y)),
                0 // TODO do przemyślenia
            );

            var cursorPosition = CombatCursorManager.Instance.MainCursor.transform.position;
            var nextTarget = cursorPosition + targetOffset;

            GetComponent<Shooter3d>().target = nextTarget;
            shooter.StartShooting();
        }
    }

    #endregion
}
