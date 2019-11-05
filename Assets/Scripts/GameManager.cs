using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cultistPrefab;
    public int initialCultistsNumber;

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

        for (int i = 0; i < initialCultistsNumber; i++)
        {
            Instantiate(cultistPrefab,Vector3.zero,Quaternion.identity);
        }
    }
}
