using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[DisallowMultipleComponent]
public class CombatSceneManager : MonoBehaviour
{

    public List<GameObject> ourCrew = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();

    public UnityEvent OnLeftButton, OnRigthButton;

    public GameObject walkTargetIndicator;
    public GameObject shootTargetIndicator;

    public Vector2 startPoint;

    public void PlaceWalkTargetIndicator() => walkTargetIndicator.transform.position = CombatSceneManager.Instance.MousePos;
    public void PlaceShootTargetIndicator() => shootTargetIndicator.transform.position = CombatSceneManager.Instance.MousePos;


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
    

    public static CombatSceneManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startPoint, .2f);
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
            if(!enemy)
            {
                continue;
            }

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
