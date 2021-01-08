using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationEnterExitController : MonoBehaviour
{
    public static Vector3 enterDirection;
    public static Vector3 exitDirection;

#pragma warning disable
    [SerializeField] private float locationRadius;
    [SerializeField] private float exitTime = 3.0f;
#pragma warning restore

    public float ExitProgress { get; private set; }
    public float ExitProgressNormalized => ExitProgress / exitTime;
    [HideInInspector] public bool isExiting = false;
    private GameObject cultLeader;

    private void OnValidate()
    {
        if (locationRadius <= 0.0f)
        {
            Debug.LogError("Location radius not set, the location will not work properly!!!");
        }
    }

    private void Awake()
    {
        EnterZone[] enterZones = FindObjectsOfType<EnterZone>();
        float[] angles = new float[enterZones.Length];
        Vector3 direction;
        for (int i = 0; i < enterZones.Length; i++)
        {
            direction = transform.position - enterZones[i].transform.position;
            direction.y = 0.0f;
            direction = direction.normalized;
            angles[i] = Vector3.Angle(enterDirection, direction);
        }

        float smallestAngle = angles[0];
        int bestIndex = 0;
        for (int i = 0; i < angles.Length; i++)
        {
            if (angles[i] < smallestAngle)
            {
                smallestAngle = angles[i];
                bestIndex = i;
            }
        }
        cultLeader = Instantiate(ApplicationManager.Instance.PrefabDatabase.cultLeader, enterZones[bestIndex].transform.position, Quaternion.identity);
        ExitZone[] exitZones = FindObjectsOfType<ExitZone>();
        foreach(var obj in exitZones)
        {
            obj.center = this;
        }
    }

    private void Update()
    {
        if (Vector3.Distance(cultLeader.transform.position, transform.position) > locationRadius || isExiting)
        {
            ExitProgress += Time.deltaTime;
        }
        else
        {
            ExitProgress -= Time.deltaTime;
        }

        ExitProgress = Mathf.Clamp(ExitProgress, 0.0f, exitTime);

        if (ExitProgress >= exitTime)
        {
            Exit();
        }
    }

    private void Exit()
    {
        Vector3 vec = cultLeader.transform.position - transform.position;
        vec.y = 0;
        exitDirection = vec.normalized;
        GameplayManager.Instance.ExitLocation();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, locationRadius);
    }
}
