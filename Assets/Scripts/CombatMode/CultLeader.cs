﻿using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine;

public class CultLeader : Character
{
    #region Variables



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
    }

    protected override void Start()
    {
        hp = 100;
        defence = 20;
        base.Start();
    }

    protected override void Update()
    {
        bool canMove = CheckState(CharacterState.CanMove);

        if (canMove && Input.GetMouseButtonDown(0))
        {
            GoToMousePosition();
        }

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

    private void OnSceneUnload(Scene scene)
    {
        if (scene.name != "MainMap" && scene.name != "MainMenu")
        {
            GameManager.Instance.ourCrew.Remove(gameObject);
            CrewSceneManager.Instance.enemies.Remove(gameObject);
        }
    }

    public void GoToMousePosition()
    {
        if (Agent.enabled && !GameManager.Gui.isMouseOver)
        {
            Agent.SetDestination(CrewSceneManager.Instance.MousePos + FormationOffset);
        }
    }

    public void AimToMousePosition()
    {
        GetComponent<Shooter>().target = CrewSceneManager.Instance.MousePos + FormationOffset;
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
}
