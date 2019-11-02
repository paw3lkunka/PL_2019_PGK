using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if( instance == null )
            {
                GameObject obj = new GameObject("GameManager");
                return instance = obj.AddComponent<GameManager>();
            }
            return instance;
        }
    }

    private void OnValidate()
    {
        if( instance == null )
        {
            instance = this;
        }
        else if( instance != this )
        {
            Debug.LogError("Duplicated GameManager");
        }
    }

    public List<GameObject> ourCrew = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();


}
