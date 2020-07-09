using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationCentre : MonoBehaviour
{
    static public Vector3 enterDirection;
    static public Vector3 exitDirection;

#pragma warning disable
    [SerializeField] private float locationRadius;
#pragma warning restore

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
        Debug.Log("Enter direction: " + enterDirection);
        Vector3 direction;
        for (int i = 0; i < enterZones.Length; i++)
        {
            direction = transform.position - enterZones[i].transform.position;
            direction.y = 0.0f;
            direction = direction.normalized;
            Debug.Log("Direction " + i + ": " + direction);
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
    }

    private void Update()
    {
        if (Vector3.Distance(cultLeader.transform.position, transform.position) > locationRadius)
        {
            Vector3 vec = cultLeader.transform.position - transform.position;
            vec.y = 0;
            exitDirection = vec.normalized;
            GameplayManager.Instance.ExitLocation();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, locationRadius);
    }
}
