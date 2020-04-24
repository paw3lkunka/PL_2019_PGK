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
public class ApplicationManager : Singleton<ApplicationManager>
{
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

    [Header("Prefab database")] // * =========================================
    public PrefabDatabase prefabDatabase;

    [Header("Rhythm System Config")] // * ===================================
    public float easyModeGoodTolerance = 0.18f;
    public float easyModeGreatTolerance = 0.1f;
    [Space]
    public float hardModeGoodTolerance = 0.28f;
    public float hardModeGreatTolerance = 0.2f;

    // Tolerances set depending on game mode selected
    public float GoodTolerance { get; private set; }
    public float GreatTolerance { get; private set; }

    // * ===== New input system ==============================================
    public NewInput Input { get; private set; }
    private PlayerInput playerInput;
    public InputSchemeEnum CurrentInputScheme { get; private set; }
    public event System.Action<PlayerInput> InputSchemeChange
    {
        add { playerInput.onControlsChanged += value; }
        remove { playerInput.onControlsChanged -= value; }
    }

    // * ===== Game state ==================================================
    public bool IsGameOver { get; private set; } = false;

    // * ===== Location variables ==========================================

    public Location currentLocation;
    public Dictionary<Location, HashSet<int>> destroyedDynamicObjects = new Dictionary<Location, HashSet<int>>();
    public HashSet<int> CurrentLocationsDestroyedDynamicObjects
    {
        get
        {
            try
            {
                return destroyedDynamicObjects[currentLocation];
            }
            catch (ArgumentNullException e)
            {
                return null;
            }
            catch (KeyNotFoundException e)
            {
                return null;
            }
        }
    }

    // * ===== Game events =================================================
    public event System.Action OnLocationEnter;
    public event System.Action OnLocationExit;
    public event System.Action OnGameOver;
    

#region MonoBehaviour

    private void Awake() 
    {
        // ? +++++ Initialize new input system +++++
        Input = new NewInput();
        playerInput = GetComponent<PlayerInput>();
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
        IsGameOver = true;
        OnGameOver?.Invoke();

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
                SceneManager.LoadScene(skipTutorial ? startScene : tutorialScene);
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
}
