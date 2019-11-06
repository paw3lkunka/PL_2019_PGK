using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject cultistPrefab;
    public GameObject guiPrefab;

    public GUI Gui { get; private set; }

    public int initialCultistsNumber;
    public int cultistNumber;


    [SerializeField]
    private float faithForKilledEnemy = 0.1f;
    [SerializeField]
    private float faithForKilledCultist = 0.2f;
    [SerializeField]
    private float faithForWoundedCultist = 0.01f;

    public float FaithForKilledEnemy { get => faithForKilledEnemy; private set => faithForKilledEnemy = value; }
    public float FaithForKilledCultist { get => faithForKilledCultist; private set => faithForKilledCultist = value; }
    public float FaithForWoundedCultist { get => faithForWoundedCultist; private set => faithForWoundedCultist = value; }



    public event System.Action OnWaterLow;
    public event System.Action OnFaithLow;
    public event System.Action OnFaithHigh;


    [Range(0, 1),SerializeField]
    private float water = 1;
    [Range(0, 1), SerializeField]
    private float faith = .5f;

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
    

    private void Awake()
    {
        ResetIndicatorsValues();
        if(Instance == null)
        {
            Instance = this;
            Initialize();
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

    public void Restart()
    {
        ResetIndicatorsValues();
        SceneManager.LoadScene(0);
        Destroy(GetComponent<GameOver>().GameOverScreenInstance);
        Initialize();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
