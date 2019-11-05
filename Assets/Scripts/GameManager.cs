using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject cultistPrefab;
    public int initialCultistsNumber;
    public int cultistNumber;


    [Range(0, 1),SerializeField]
    private float water = 1;
    [Range(0, 1), SerializeField]
    private float faith = .6f;

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
        if(Instance == null)
        {
            Instance = this;
            Initialize();
        }
    }

    private void Initialize()
    {
        DontDestroyOnLoad(gameObject);
        cultistNumber = initialCultistsNumber;

        for (int i = 0; i < initialCultistsNumber; i++)
        {
            Instantiate(cultistPrefab,Vector3.zero,Quaternion.identity);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
        water = 1f;
        faith = 0.6f;
        Awake();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
