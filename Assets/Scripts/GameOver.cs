using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject screenPrefab;
    private GameObject gameOverScreenInstance = null;
    public GameObject GameOverScreenInstance 
    {
        get => gameOverScreenInstance;
        set => gameOverScreenInstance = value;
    }

    void Start() 
    {
        gameOverScreenInstance = null;
    }

    void LateUpdate()
    {
        if(GameManager.Instance.cultistNumber <= 0 && gameOverScreenInstance == null)
        {
            Render();
        }
    }
    
    void Render()
    {
        gameOverScreenInstance = Instantiate(screenPrefab);
    }
}
