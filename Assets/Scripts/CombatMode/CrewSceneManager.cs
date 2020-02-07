using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class CrewSceneManager : MonoBehaviour
{
    #region Variables

    public static CrewSceneManager Instance { get; private set; }

    public List<GameObject> enemies = new List<GameObject>();

    public UnityEvent OnLeftButton, OnRightButton;

    public GameObject walkTargetIndicator;
    public GameObject shootTargetIndicator;

    public Vector2 startPoint;

    public bool combatMode = true;

    public NewInput input;

    public Transform cursorPrefab;
    private Transform cursorInstance;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        input = GameManager.Instance.input;
        //cursorInstance = Instantiate(cursorPrefab, new Vector3(startPoint.x, startPoint.y), Quaternion.identity);
        Instance = this;
    }

    private void OnEnable()
    {

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnLeftButton.Invoke();
        }

        if (Input.GetMouseButtonDown(1) && combatMode)
        {
            OnRightButton.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startPoint, .2f);
    }

    #endregion

    #region Component

    public Vector2 MousePos => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    public void PlaceWalkTargetIndicator()
    {
        walkTargetIndicator.transform.position = MousePos;
    }

    public void PlaceShootTargetIndicator()
    {
        if (combatMode)
        {
            shootTargetIndicator.transform.position = MousePos;
        }
    }

    #endregion
}

public static class Extensions
{
    public static (GameObject, float) NearestFrom(this List<GameObject> objs, Vector2 from)
    {
        GameObject target = null;
        float minimum = float.PositiveInfinity;

        foreach (GameObject enemy in objs)
        {
            if (!enemy)
            {
                continue;
            }

            float distance = Vector2.Distance(from, enemy.transform.position);
            if (distance < minimum)
            {
                target = enemy;
                minimum = distance;
            }
        }

        return (target, minimum);
    }
}
