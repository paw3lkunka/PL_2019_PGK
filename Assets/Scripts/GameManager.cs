using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public enum InputSchemeEnum
{
    MouseKeyboard,
    Gamepad,
    JoystickKeyboard,
    Touchscreen
}

public class GameManager : MonoBehaviour
{
    #region Variables

    public static GameManager Instance { get; private set; }

#pragma warning disable    
    [SerializeField] private bool skipTutorial = false;
    [SerializeField] private bool cheats = true;
    [SerializeField] private bool debug = false;
#pragma warning restore

    public bool SkipTutorial
    {
        get => skipTutorial;
        set => skipTutorial = value;
    }
    public bool Cheats
    {
        get => cheats;
        set => cheats = value;
    }    
    public bool Debug
    {
        get => debug;
        set => debug = value;
    }    
    public GameObject cultistPrefab;
    public GameObject leaderPrefab;
    public GameObject guiPrefab;
    public EventSystem eventSystem;

    public GameObject gameOverScreenPrefab;
    private GameObject gameOverScreenInstance = null;

    public GameObject pauseScreen;

    public static GUI Gui { get; private set; }

    public int initialCultistsNumber;
    /// <summary>
    /// Number of cultists (with leader)
    /// </summary>
    public int cultistNumber;

    public float faithForKilledEnemy = 0.01f;
    public float faithForKilledCultist = 0.02f;
    public float faithForWoundedCultist = 0.001f;

    [Header("Global rhythm system properties")]
    [HideInInspector] public float goodTolerance;
    [HideInInspector] public float greatTolerance;

    public float easyModeGoodTolerance = 0.18f;
    public float easyModeGreatTolerance = 0.1f;

    public float hardModeGoodTolerance = 0.28f;
    public float hardModeGreatTolerance = 0.2f;

    public float FaithForKilledEnemy { get => faithForKilledEnemy; set => faithForKilledEnemy = value; }
    public float FaithForKilledCultist { get => faithForKilledCultist; set => faithForKilledCultist = value; }
    public float FaithForWoundedCultist { get => faithForWoundedCultist; set => faithForWoundedCultist = value; }

    public bool mapGenerated = false;

    public event System.Action OnLocationEnter;
    public event System.Action OnLocationExit;
    public event System.Action OnGameOver;

    public GameObject GameOverScreenInstance
    {
        get => gameOverScreenInstance;
        set => gameOverScreenInstance = value;
    }

    [Range(0, 1), SerializeField]
    private float water = 1.0f;
    [Range(0, 1), SerializeField]
    private float faith = 0.5f;

    private float oldWater;
    private float oldFaith;

    public float FaithUsageFactor = 0.0002f;
    public float WaterUsageFactor = 0.0001f;

    public float LowWaterLevel = 0.2f;
    public float LowFaithLevel = 0.2f;
    public float HighFaithLevel = 0.7f;
    public float FanaticFaithLevel = 0.9f;

    public event System.Action LowWaterLevelStart;
    public event System.Action LowWaterLevelEnd;

    public event System.Action LowFaithLevelStart;
    public event System.Action LowFaithLevelEnd;

    public event System.Action HighFaithLevelStart;
    public event System.Action HighFaithLevelEnd;

    public event System.Action FanaticStart;
    public event System.Action FanaticEnd;

    public float Water
    {
        get => water;
        set => water = Mathf.Clamp(value, 0, 1);
    }

    public float Faith
    {
        get => faith;
        set => faith = Mathf.Clamp(value, 0, 1);
    }

    public Vector2 savedPosition;

    /// <summary>
    /// List of cultists (with leader at [0] )
    /// </summary>
    public List<GameObject> ourCrew;

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
            catch(Exception exc)
            {
                if (exc is KeyNotFoundException || exc is ArgumentNullException)
                {
                    return null;
                }
                else
                {
                    throw exc;
                }
            }
        }
    }

    public List<Location> ShrinesVisited { get; set; }

    // Input related global variables
    #region Input Fields

    [HideInInspector]
    public NewInput input;
    
    private PlayerInput playerInput;
    [HideInInspector]
    public event Action<PlayerInput> InputSchemeChange
    {
        add { playerInput.onControlsChanged += value; }
        remove { playerInput.onControlsChanged -= value; }
    }

    public InputSchemeEnum currentInputScheme;

    #endregion
    
    private bool firstLoad = false;
    public bool won = false;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        oldWater = water;
        oldFaith = faith;

        gameOverScreenInstance = null;
        input = new NewInput();
        playerInput = GetComponent<PlayerInput>();
        ShrinesVisited = new List<Location>();

        ResetIndicatorsValues();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoad;
        }
    }

    private void OnEnable()
    {
        InputSchemeChange += OnInputSchemeChange;
    }

    private void OnDisable()
    {
        InputSchemeChange -= OnInputSchemeChange;
    }

    private void Update()
    {
        if (cultistNumber <= 0 && gameOverScreenInstance == null)
        {
            GameOver();
        }

        if (water < LowWaterLevel && oldWater >= LowWaterLevel)
            LowWaterLevelStart?.Invoke();

        if (water > LowWaterLevel && oldWater <= LowWaterLevel)
            LowWaterLevelEnd?.Invoke();

        if (faith < LowFaithLevel && oldFaith >= LowFaithLevel)
            LowFaithLevelStart?.Invoke();

        if (faith > LowFaithLevel && oldFaith <= LowFaithLevel)
            LowFaithLevelEnd?.Invoke();

        if (faith > HighFaithLevel && oldFaith <= HighFaithLevel)
            HighFaithLevelStart?.Invoke();

        if (faith < HighFaithLevel && oldFaith >= HighFaithLevel)
            HighFaithLevelEnd?.Invoke();

        if (faith > FanaticFaithLevel && oldFaith <= FanaticFaithLevel)
            FanaticStart?.Invoke();

        if (faith < FanaticFaithLevel && oldFaith >= FanaticFaithLevel)
            FanaticEnd?.Invoke();

        oldFaith = faith;
        oldWater = water;
    }

    #endregion

    #region Component

    private void Initialize()
    {
        DontDestroyOnLoad(eventSystem);
        Gui = Instantiate(guiPrefab, new Vector3(0, 0, -10), Quaternion.identity).GetComponent<GUI>();
        Gui.gameObject.SetActive(false);
        ourCrew = new List<GameObject>();
        Destroy(gameOverScreenInstance);
        RestartCultists();
    }

    public void LoadCustomScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void RestartCultists()
    {
        cultistNumber = initialCultistsNumber;
#pragma warning disable
        Instantiate(leaderPrefab, Vector3.zero, Quaternion.identity);
        cultistNumber += 1;
        for (int i = 0; i < initialCultistsNumber; i++)
        {
            Instantiate(cultistPrefab, Vector3.zero, Quaternion.identity);
        }
#pragma warning restore
    }

    public void RemoveCultistsFromCrew()
    {
        for(int i = ourCrew.Count - 1; i >= 0; --i)
        {
            Destroy(ourCrew[i]);
        }
        ourCrew.Clear();
    }

    private void ResetIndicatorsValues()
    {
        water = 1.0f;
        faith = 0.5f;
        ShrinesVisited.Clear();
        RemoveCultistsFromCrew();
    }

    public void GameOver()
    {
        OnGameOver?.Invoke();
        if (won)
        {
            Destroy(Gui.transform.root.gameObject);
            SceneManager.LoadScene("EndgameScene");
        }
        else
        {
            gameOverScreenInstance = Instantiate(gameOverScreenPrefab);
        }
        Cursor.visible = true;
    }

    public void HardModeStart()
    {
        goodTolerance = hardModeGoodTolerance;
        greatTolerance = hardModeGreatTolerance;

        Gui.gameObject.SetActive(true);
        SceneManager.LoadScene(SkipTutorial ? "MainMap" : "TutorialNew");
    }

    public void EasyModeStart()
    {
        goodTolerance = easyModeGoodTolerance;
        greatTolerance = easyModeGreatTolerance;

        Gui.gameObject.SetActive(true);
        SceneManager.LoadScene(SkipTutorial ? "MainMap" : "TutorialNew");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ToRageMode()
    {
        FaithForKilledEnemy *= 2;
    }

    public void ToNormalMode()
    {
        FaithForKilledEnemy /= 2;
    }

    public void BackToMenu()
    {
        ResetIndicatorsValues();
        Destroy(gameOverScreenInstance);
        Gui.gameObject.SetActive(false);
        RestartCultists();
        SceneManager.LoadScene("MainMenu");
    }


    #endregion

    #region event invokes

    public void OnLocationExitInvoke() => OnLocationExit?.Invoke();
    public void OnLocationEnterInvoke() => OnLocationEnter?.Invoke();

    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if(!firstLoad && scene.name.Equals("MainMenu"))
        {
            Initialize();
            firstLoad = true;
        }
    }

    #endregion

    #region Input

    private void OnInputSchemeChange(PlayerInput obj)
    {
        currentInputScheme = (InputSchemeEnum)(Enum.Parse(typeof(InputSchemeEnum), obj.currentControlScheme));
    }

    #endregion
}
