using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject screenPrefab;
    private GameObject gameOver = null;


    void LateUpdate()
    {
        if(GameManager.Instance.initialCultistsNumber <= 0 && gameOver == null)
        {
            Render();
        }
    }
    
    void Render()
    {
        gameOver = Instantiate(screenPrefab);
    }
}
