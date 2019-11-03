using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{

    public List<GameObject> ourCrew = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();


    public StartArena startArea;

    public MouseInput levelInput;

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
        instance = this;
    }

    

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    public Vector2 MousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition);
}

public static class Extensions
{
    public static (GameObject, float) NearestFrom(this List<GameObject> objs, Vector2 from)
    {
        GameObject target = null;
        float minimum = float.PositiveInfinity;

        foreach (GameObject enemy in objs)
        {
            float distance = Vector2.Distance(from, enemy.transform.position);
            if ( distance < minimum)
            {
                target = enemy;
                minimum = distance;
            }
        }

        return (target, minimum);
    }
}
