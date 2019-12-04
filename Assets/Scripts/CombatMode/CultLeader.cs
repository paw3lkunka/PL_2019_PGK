using System.Collections;
using System.Collections.Generic;
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
        shooter = GetComponent<Shooter>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
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

    protected override void Start()
    {
        base.Start();
        RhythmController.Instance.OnRageModeStart += ToRageMode;
        RhythmController.Instance.OnRageModeEnd += ToNormalMode;
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
            agent.Warp(CombatSceneManager.Instance.startPoint + FormationOffset);
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
}
