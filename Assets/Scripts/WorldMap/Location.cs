using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SphereCollider))]
public class Location : MonoBehaviour
{
#pragma warning disable
    [field: Tooltip("Name of location should be equal to location's scene name")]
    [field: GUIName("SceneName"), SerializeField] 
    public string SceneName { get; private set; }
    [SerializeField] private float locationResetTime = 120.0f;
#pragma warning restore

    private bool isEntering = false;

    [Header("Saved in prefs")]
    public float timeToRefill;
    public bool visited = false;

    [Header("Generation")]
    public MapGenerator generatedBy;
    public int id;

    [HideInInspector]
    public float radius;

    private string PrefsKey(string key) => "Loc" + id + key;

    public void SaveState()
    {
        if (visited)
        {
            PlayerPrefs.SetInt(PrefsKey("v"), 1);
        }

        if (timeToRefill > 0)
        {
            PlayerPrefs.SetFloat(PrefsKey("t"), timeToRefill);
        }
    }

    public void LoadState()
    {
        if (PlayerPrefs.HasKey(PrefsKey("v")))
        {
            visited = PlayerPrefs.GetInt(PrefsKey("v")) != 0;
        }

        if (PlayerPrefs.HasKey(PrefsKey("t")))
        {
            timeToRefill = PlayerPrefs.GetFloat(PrefsKey("t"));
        }
    }

    public void ClearSave()
    {
        PlayerPrefs.DeleteKey(PrefsKey("v"));
        PlayerPrefs.DeleteKey(PrefsKey("t"));
    }

    private void Awake()
    {
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider)
        {
            radius = sphereCollider.radius;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            if (WorldSceneManager.Instance.CanEnterLocations)
            {
                isEntering = true;
                EnterExitLocationManager.Instance.IsEnteringExiting = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            isEntering = false;
            EnterExitLocationManager.Instance.IsEnteringExiting = false;
        }
    }

    private void Update()
    {
        if (timeToRefill > 0)
        {
            timeToRefill -= Time.deltaTime;
        }

        if (EnterExitLocationManager.Instance.EnterExitProgressNormalized >= 1.0f && isEntering)
        {
            EnterLocation();
        }
    }

    private void EnterLocation()
    {
        Vector3 vec = transform.position - WorldSceneManager.Instance.Leader.transform.position;
        vec.y = 0;
        LocationManager.enterDirection = vec.normalized;
        GameplayManager.Instance.EnterLocation(this);

        visited = true;
        timeToRefill = locationResetTime;

        generatedBy?.SaveState();
    }
}
