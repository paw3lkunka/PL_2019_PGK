using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    public List<GameObject> ourCrew = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();

    public UnityEvent OnLeftButton, OnRigthButton;

    public GameObject walkTargetIndicator;
    public GameObject shootTargetIndicator;

    public StartArena startArea;
    public Collider2D[] escapeAreas; 

    public void PlaceWalkTargetIndicator() => walkTargetIndicator.transform.position = MousePos;
    public void PlaceShootTargetIndicator() => shootTargetIndicator.transform.position = MousePos;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnLeftButton.Invoke();
        }

        if (Input.GetMouseButtonDown(1))
        {
            OnRigthButton.Invoke();
        }
    }

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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    private void OnValidate()
    {
        instance = this;
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
