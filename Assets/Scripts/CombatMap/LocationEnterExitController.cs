using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationEnterExitController : MonoBehaviour
{
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
        
        ExitZone[] exitZones = FindObjectsOfType<ExitZone>();
        foreach(var obj in exitZones)
        {
            obj.center = this;
        }
    }

    private void Update()
    {
        if (Vector3.Distance(LocationManager.Instance.cultLeader.transform.position, transform.position) > locationRadius || isExiting)
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
        LocationManager.exitDirection = vec.normalized;
        GameplayManager.Instance.ExitLocation();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, locationRadius);
    }
}
