﻿using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine;

[RequireComponent(typeof(Shooter))]
public class Cultist : Character
{
    private Shooter shooter;
    public bool fanaticMode = false;
    public bool isFanatic = false;

    #region Mono behaviour functions
    protected override void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        SceneManager.sceneUnloaded += OnSceneUnload;
        GameManager.Instance.OnGameOver += OnGameOver;

        DontDestroyOnLoad(gameObject);
        GameManager.Instance.ourCrew.Add(gameObject);
        gameObject.transform.position = GameManager.Instance.ourCrew[0].transform.position + new Vector3(FormationOffset.x, FormationOffset.y);

        string sceneName = SceneManager.GetActiveScene().name;
        if ( sceneName == "MainMap" || sceneName == "MainMenu")
        {
            gameObject.SetActive(false);
        }

        base.Awake();
        shooter = GetComponent<Shooter>();

        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }

    protected override void Start()
    {
        base.Start();
        try
        {
            RhythmController.Instance.OnRageModeStart += ToRageMode;
        }
        catch (System.NullReferenceException) { }

        try
        {
            RhythmController.Instance.OnRageModeEnd += ToNormalMode;
        }
        catch (System.NullReferenceException) { }
    }

    protected override void Update()
    {
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

        (GameObject,float) nearest = CrewSceneManager.Instance.enemies.NearestFrom(transform.position);
        bool canMove = CheckState(CharacterState.CanMove);
        bool canAttack = CrewSceneManager.Instance.combatMode && CheckState(CharacterState.CanAttack);

        if (isFanatic)
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
            RhythmController.Instance.OnRageModeStart -= ToRageMode;
        }
        catch (System.NullReferenceException) { }

        try
        {
            RhythmController.Instance.OnRageModeEnd -= ToNormalMode;
        }
        catch (System.NullReferenceException) { }
    }
    #endregion

    #region Behaviour

    public void StandardBehaviour(GameObject enemy, float distanceToEnemy, bool canMove, bool canAttack)
    {

        if (!canAttack || distanceToEnemy > shooter.range)
        {
            shooter.StopShooting();
        }

        if (canMove && Input.GetMouseButtonDown(0))
        {
            GoToMousePosition();
        }

        if (canAttack && Input.GetMouseButtonDown(1))
        {
            AimToMousePosition();
            shooter.StartShooting();
        }
    }

    public void FanaticBehaviour(GameObject enemy, float distanceToEnemy, bool canMove, bool canAttack)
    {
        if ( canAttack && distanceToEnemy <= shooter.range)
        {
            Vector3 target = enemy.transform.position;
            Agent.SetDestination(target);
            shooter.target = target;
            shooter.StartShooting();

        }
        else
        {
            shooter.StopShooting();
            isFanatic = false;
        }
    }

    #endregion

    #region Rage mode handling

    private void ToRageMode()
    {
        shooter.baseDamage *= 1.5f;
        Agent.speed *= 1.1f;
        defence = .5f;
    }

    private void ToNormalMode()
    {
        shooter.baseDamage /= 1.5f;
        Agent.speed /= 1.1f;
        defence = 0;
    }
    #endregion

    #region Control

    public void GoToMousePosition()
    {
        if(Agent.enabled && !GameManager.Gui.isMouseOver)
        {
            Agent.SetDestination(CrewSceneManager.Instance.MousePos + FormationOffset);
        }
    }

    public void AimToMousePosition()
    {
        Vector2 offset = new Vector2
        (
            Random.Range(Mathf.Min(0, FormationOffset.x), Mathf.Max(0, FormationOffset.x)),
            Random.Range(Mathf.Min(0, FormationOffset.y), Mathf.Max(0, FormationOffset.y))
        );
        GetComponent<Shooter>().target = CrewSceneManager.Instance.MousePos + offset;
    }
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
        if (scene.name == "MainMap" || scene.name == "MainMenu")
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            Agent.Warp(CrewSceneManager.Instance.startPoint + FormationOffset);
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
}
