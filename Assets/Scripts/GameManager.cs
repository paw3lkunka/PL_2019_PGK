using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool skipTutorial = false;
    public bool SkipTutorial { get => skipTutorial; set => skipTutorial = value; }

    public GameObject cultistPrefab;
    public GameObject guiPrefab;

    public GameObject gameOverScreenPrefab;
    private GameObject gameOverScreenInstance = null;

    static public GUI Gui { get; private set; }

    public int initialCultistsNumber;
    public int cultistNumber;


    [SerializeField]
    private float faithForKilledEnemy = 0.1f;
    [SerializeField]
    private float faithForKilledCultist = 0.2f;
    [SerializeField]
    private float faithForWoundedCultist = 0.01f;

    [Header("Global rhythm system properties")]
    public float goodTolerance = 0.18f;
    public float greatTolerance = 0.1f;

    public float easyModeGoodTolerance = 0.18f;
    public float easyModeGreatTolerance = 0.1f;

    public float hardModeGoodTolerance = 0.28f;
    public float hardModeGreatTolerance = 0.2f;

    public float FaithForKilledEnemy { get => faithForKilledEnemy; set => faithForKilledEnemy = value; }
    public float FaithForKilledCultist { get => faithForKilledCultist; set => faithForKilledCultist = value; }
    public float FaithForWoundedCultist { get => faithForWoundedCultist; set => faithForWoundedCultist = value; }

    public event System.Action OnGameOver;

    public GameObject GameOverScreenInstance
    {
        get => gameOverScreenInstance;
        set => gameOverScreenInstance = value;
    }
    
    [Range(0, 1),SerializeField]
    private float water = 1.0f;
    [Range(0, 1), SerializeField]
    private float faith = 0.5f;

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

    public static GameManager Instance { get; private set; }
    
    public Vector2 savedPosition;

    private void Awake()
    {
        gameOverScreenInstance = null;

        ResetIndicatorsValues();
        if(Instance == null)
        {
            Instance = this;
            Initialize();
        }
        RhythmController.Instance.OnRageModeStart += ToRageMode;
        RhythmController.Instance.OnRageModeEnd += ToNormalMode;

    }

    private void Update()
    {

        if (cultistNumber <= 0 && gameOverScreenInstance == null)
        {
            GameOver();
        }

        if( Input.GetKeyDown(KeyCode.Escape))
        {
            GameOver();
        }
    }

    private void Initialize()
    {
        DontDestroyOnLoad(gameObject);
        Gui = Instantiate(guiPrefab).GetComponent<GUI>();
        Gui.gameObject.SetActive(false);

        for (int i = 0; i < initialCultistsNumber; i++)
        {
            Instantiate(cultistPrefab,Vector3.zero,Quaternion.identity);
        }
    }

    private void ResetIndicatorsValues()
    {
        cultistNumber = initialCultistsNumber;
        water = 1.0f;
        faith = 0.5f;
    }

    void GameOver()
    {
        if( OnGameOver != null )
        {
            OnGameOver();
        }

        gameOverScreenInstance = Instantiate(gameOverScreenPrefab);
    }

    public void Restart()
    {
        ResetIndicatorsValues();
        SceneManager.LoadScene("MainMenu");
        Destroy(Gui.gameObject);
        Gui = null;
        Destroy(GameOverScreenInstance);
        Initialize();
    }

    public void HardModeStart()
    {
        goodTolerance = hardModeGoodTolerance;
        greatTolerance = hardModeGreatTolerance;

        Gui.gameObject.SetActive(true);
        SceneManager.LoadScene(SkipTutorial ? "MainMap" : "Tutorial");
    }

    public void EasyModeStart()
    {
        goodTolerance = easyModeGoodTolerance;
        greatTolerance = easyModeGreatTolerance;

        Gui.gameObject.SetActive(true);
        SceneManager.LoadScene(SkipTutorial ? "MainMap" : "Tutorial");
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void ToRageMode()
    {
       FaithForKilledEnemy *= 2;
    }


    private void ToNormalMode()
    {
        FaithForKilledEnemy /= 2;
    }
}
