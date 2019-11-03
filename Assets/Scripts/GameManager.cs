using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cultistPrefab;
    public int initialCultistsNumber;

    [Range(0, 1)]
    public float water = 1;
    [Range(0, 1)]
    public float faith = .5f;

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
            Debug.Log("LOOOOOOOOOOOOOOOOOOOOOOP");
            Instantiate(cultistPrefab);
        }
    }
}
