using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

public enum GameMode { Normal, Debug };
public enum Difficulty { Easy, Hard };
public enum InputSchemeEnum
{
    MouseKeyboard,
    Gamepad,
    JoystickKeyboard,
    Touchscreen
};

/// <summary>
/// Handles global game configuration, provides access to scene persistent components.
/// Should persist throughout whole application.
/// </summary>
public class ApplicationManager : Singleton<ApplicationManager, AllowLazyInstancing>
{
    public Difficulty defaultDifficulty; 

    [Header("Scene configuration")] // * =====================================
    public string menuScene;
    public string worldMapScene;
    public string tutorialScene;
    public string endScene;
    public string gameOverScene;
    public string debugScene;

    [Header("Testing and debugging")] // * ===================================
    public bool skipTutorial = false;
    public bool enableCheats = false;
    public bool debugOverlay = false;

    //[Header("Prefab database")] // * =========================================
    public PrefabDatabase PrefabDatabase { get; private set; }

    [Header("Rhythm System Config")] // * ===================================
    public float easyModeGoodTolerance = 0.18f;
    public float easyModeGreatTolerance = 0.1f;
    [Space]
    public float hardModeGoodTolerance = 0.28f;
    public float hardModeGreatTolerance = 0.2f;

    // Tolerances set depending on game mode selected
    public float GoodTolerance { get; private set; } = float.NaN;
    public float GreatTolerance { get; private set; } = float.NaN;

    // * ===== New input system ==============================================
    public NewInput Input { get; private set; }
    private PlayerInput playerInput;
    [field: SerializeField, GUIName("CurrentInputScheme")]
    public InputSchemeEnum CurrentInputScheme { get; private set; }
    public event Action<PlayerInput> InputSchemeChange
    {
        add { playerInput.onControlsChanged += value; }
        remove { playerInput.onControlsChanged -= value; }
    }

    // * ===== Game over event ===============================================
#pragma warning disable
    public event Action NewGameEvent;
    public event Action BackToMenuEvent;
    public event Action GameOverEvent;
    public event Action WinGameEvent;
#pragma warning restore

    #region MonoBehaviour

    protected override void Awake() 
    {
        base.Awake();
        // ? +++++ Initialize new input system +++++
        Input = new NewInput();
        playerInput = GetComponent<PlayerInput>();
        if (SceneManager.GetActiveScene().name != menuScene)
        {
            SetDifficulty(defaultDifficulty);
        }
        PrefabDatabase = (PrefabDatabase)Resources.Load("PrefabDatabase");
    }

    private void OnEnable()
    {
        InputSchemeChange += OnInputSchemeChange;
    }

    private void OnDisable()
    {
        InputSchemeChange -= OnInputSchemeChange;
    }

    #endregion

    #region ManagerMethods

    public void GameOver(bool won = false)
    {
        GameOverEvent?.Invoke();

        if (won)
        {
            SceneManager.LoadScene(endScene);
        }
        else
        {
            SceneManager.LoadScene(gameOverScene);
            // TODO: Cursor visibility handling
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(menuScene);
    }

    public void StartGame(GameMode mode, Difficulty difficulty)
    {
        

        SetDifficulty(difficulty);
        switch(mode)
        {
            case GameMode.Debug:
                // Load appropriate scene
                SceneManager.LoadScene(debugScene);
                break;

            case GameMode.Normal:
                // Load appropriate scene
                SceneManager.LoadScene(skipTutorial ? worldMapScene : tutorialScene);
                break;
        }
    }

    public void SetDifficulty(Difficulty difficulty)
    {
        switch(difficulty)
        {
            case Difficulty.Easy:
                // Set tolerances for rhythm system
                GoodTolerance = easyModeGoodTolerance;
                GreatTolerance = easyModeGreatTolerance;
                break;

            case Difficulty.Hard:
                // Set tolerances for rhythm system
                GoodTolerance = hardModeGoodTolerance;
                GreatTolerance = hardModeGreatTolerance;
                break;
        }
    }

#endregion

#region EventHandlers

    private void OnInputSchemeChange(PlayerInput obj)
    {
        CurrentInputScheme = (InputSchemeEnum)(Enum.Parse(typeof(InputSchemeEnum), obj.currentControlScheme));
    }

#endregion
}
