using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static bool first = true;

    public GameObject cultistPrefab;
    public GameObject guiPrefab;

    public GameObject gameOverScreenPrefab;
    private GameObject gameOverScreenInstance = null;

    public GUI Gui { get; private set; }

    public int initialCultistsNumber;
    public int cultistNumber;


    [SerializeField]
    private float faithForKilledEnemy = 0.1f;
    [SerializeField]
    private float faithForKilledCultist = 0.2f;
    [SerializeField]
    private float faithForWoundedCultist = 0.01f;

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

        if(first)
        {
            first = false;
            SceneManager.LoadScene(4);
        }

    }

    private void Update()
    {
        if (cultistNumber <= 0 && gameOverScreenInstance == null)
        {
            GameOver();
        }
    }

    private void Initialize()
    {
        DontDestroyOnLoad(gameObject);
        Gui = Instantiate(guiPrefab).GetComponent<GUI>();

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
        SceneManager.LoadScene(0);
        Destroy(GameOverScreenInstance);
        Initialize();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
