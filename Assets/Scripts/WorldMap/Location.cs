using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Location : MonoBehaviour
{
#pragma warning disable
    [field: Tooltip("Name of location should be equal to location's scene name")]
    [field: GUIName("SceneName"), SerializeField] 
    public string SceneName { get; private set; }
    [SerializeField] private float enterDelay = 3.0f;
    [SerializeField] private float locationResetTime = 120.0f;
    [SerializeField] private GameObject locationVisitedIndicator;
#pragma warning restore

    [Header("Saved in prefs")]
    public float timeToRefill;
    public bool visited = false;

    [Header("Generation")]
    public MapGenerator generatedBy;
    public int id;

    private float timeElapsedToEnter = 0.0f;
    private Image enterProgressBar;
    private GameObject cultistLeader;

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
        enterProgressBar = GetComponentInChildren<Image>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            if (WorldSceneManager.Instance.CanEnterLocations)
            {
                cultistLeader = other.gameObject;
                StartCoroutine(EnterLocationRoutine());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            StopAllCoroutines();
            timeElapsedToEnter = 0.0f;
        }
    }

    private void Update()
    {
        enterProgressBar.fillAmount = timeElapsedToEnter / enterDelay;

        if (timeToRefill > 0)
        {
            timeToRefill -= Time.deltaTime;
        }

        if (visited)
        {
            locationVisitedIndicator.SetActive(true);
        }
        else
        {
            locationVisitedIndicator.SetActive(false);
        }
    }

    private IEnumerator EnterLocationRoutine()
    {
        while(true)
        {
            timeElapsedToEnter += Time.deltaTime;

            if (timeElapsedToEnter >= enterDelay)
            {
                Vector3 vec = transform.position - cultistLeader.transform.position;
                vec.y = 0;
                LocationCentre.enterDirection = vec.normalized;
                GameplayManager.Instance.EnterLocation(this);

                visited = true;
                timeToRefill = locationResetTime;

                generatedBy?.SaveState();
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
