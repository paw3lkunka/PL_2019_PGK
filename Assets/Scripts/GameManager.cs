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


    public Vector2 MousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition);


}

public static class Extensions
{
    public static GameObject GetNearestFrom(this List<GameObject> objs, Vector2 from)
    {
        GameObject target = null;
        float distance = float.PositiveInfinity;
        foreach (GameObject enemy in objs)
        {
            if (Vector2.Distance(from, enemy.transform.position) < distance)
            {
                target = enemy;
            }
        }
        return target;
    }

    public static KeyValuePair<GameObject, float> GetDistanceFromNearest(this List<GameObject> objs, Vector2 from)
    {
        var nearest = objs.GetNearestFrom(from);
        if(!nearest)
        {
            return new KeyValuePair<GameObject, float>(null, 0.0f);
        }

        var distance = Vector2.Distance(nearest.transform.position, from);
        
        return new KeyValuePair<GameObject, float>(nearest, distance);
    }
}
