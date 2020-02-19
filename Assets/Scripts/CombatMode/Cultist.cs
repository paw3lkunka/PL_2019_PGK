﻿using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Shooter))]
public class Cultist : Character
{
    #region Variables

    private Shooter shooter;
    public bool isFanatic;
    public bool fanaticState;

    [Range(0, 0.001f)] public float fanaticStateEnterChance = 0.05f, fanaticStateExitChance = 0.01f;
    public float fanaticAimDebuff;

    private NewInput input;
    private bool canMove;
    private bool canAttack;

    #endregion

    #region MonoBehaviour

    protected override void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        SceneManager.sceneUnloaded += OnSceneUnload;
        GameManager.Instance.OnGameOver += OnGameOver;

        DontDestroyOnLoad(gameObject);
        GameManager.Instance.ourCrew.Add(gameObject);
        gameObject.transform.position = GameManager.Instance.ourCrew[0].transform.position + new Vector3(FormationOffset.x, FormationOffset.y);

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "MainMap" || sceneName == "MainMenu")
        {
            gameObject.SetActive(false);
        }

        base.Awake();
        shooter = GetComponent<Shooter>();

        Agent.updateRotation = false;
        Agent.updateUpAxis = false;

        input = GameManager.Instance.input;
    }

    protected override void Start()
    {
        base.Start();

        isFanatic = GameManager.Instance.Faith > GameManager.Instance.FanaticFaithLevel;
        fanaticState = false;

        try
        {
            RhythmController.Instance.OnRageModeStart += EnterRageMode;
        }
        catch (System.NullReferenceException) { }

        try
        {
            RhythmController.Instance.OnRageModeEnd += ExitRageMode;
        }
        catch (System.NullReferenceException) { }

        GameManager.Instance.FanaticStart += EnterFanaticMode;
        GameManager.Instance.FanaticEnd += ExitFanaticMode;
    }

    protected void OnEnable()
    {
        if(input != null)
        {
            input.Gameplay.SetWalkTarget.performed += GoToCursorPosition;
            input.CombatMode.SetShootTarget.performed += AimToCursorPosition;
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


        if (CrewSceneManager.Instance.combatMode)
        {
            if (RhythmController.Instance.Combo >= 1)
            {
                SetStateOn(CharacterState.CanAttack);
            }
            else
            {
                SetStateOff(CharacterState.CanAttack);
            }

            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            // Hide HealthBar on neutral scenes
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }

        (GameObject, float) nearest = CrewSceneManager.Instance.enemies.NearestFrom(transform.position);
        canMove = CheckState(CharacterState.CanMove);
        canAttack = CrewSceneManager.Instance.combatMode && CheckState(CharacterState.CanAttack);

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
            RhythmController.Instance.OnRageModeStart -= EnterRageMode;
        }
        catch (System.NullReferenceException) { }

        try
        {
            RhythmController.Instance.OnRageModeEnd -= ExitRageMode;
        }
        catch (System.NullReferenceException) { }

        GameManager.Instance.FanaticStart -= EnterFanaticMode;
        GameManager.Instance.FanaticEnd -= ExitFanaticMode;
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
        shooter.baseDamage *= 1.5f;
        Agent.speed *= 1.1f;
        defence = .5f;
    }

    private void ExitRageMode()
    {
        shooter.baseDamage /= 1.5f;
        Agent.speed /= 1.1f;
        defence = 0;
    }

    private void EnterFanaticMode() => isFanatic = true;
    private void ExitFanaticMode() => isFanatic = false;

    #endregion

    #region Taking damage

    public override void TakeDamage(int damage)
    {
        GameManager.Instance.Faith -= GameManager.Instance.FaithForWoundedCultist;
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        GameManager.Instance.OnGameOver -= OnGameOver;
        base.Die();

        float lossedFaith = GameManager.Instance.FaithForKilledCultist;
        GameManager.Instance.Faith -= lossedFaith;
        fatihTextEemitter.Emit("-" + (int)(lossedFaith * 100), Color.green, 3);
        GameManager.Instance.cultistNumber--;
    }

    #endregion

    #region Event listeners

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
                Agent.Warp(CrewSceneManager.Instance.startPoint + FormationOffset);
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
            CrewSceneManager.Instance.enemies.Remove(gameObject);
        }
    }

    /// <summary>
    /// GameOverEvent listener
    /// </summary>
    private void OnGameOver()
    {
        Destroy(gameObject);
        GameManager.Instance.OnGameOver -= OnGameOver;
    }

    #endregion

    #region misc
    /// <summary>
    /// Returns position in formation from formation centre
    /// </summary>
    public Vector2 FormationOffset
    {
        get
        {
            int index = GameManager.Instance.ourCrew.IndexOf(gameObject);
            switch (index % 8 + 1)
            {
                default:
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

    #endregion

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
        if(canAttack)
        {
            var targetOffset = new Vector2
            (
                Random.Range(Mathf.Min(0, FormationOffset.x), Mathf.Max(0, FormationOffset.x)),
                Random.Range(Mathf.Min(0, FormationOffset.y), Mathf.Max(0, FormationOffset.y))
            );

            var cursorPosition = CrewSceneManager.Instance.cursorInstance.position;
            var nextTarget = new Vector2(cursorPosition.x, cursorPosition.y) + targetOffset;

            GetComponent<Shooter>().target = nextTarget;
            shooter.StartShooting();
        }
    }

    #endregion
}
