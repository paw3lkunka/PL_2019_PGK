using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Location : MonoBehaviour
{
#pragma warning disable
    [Tooltip("Name of location should be equal to location's scene name")]
    [SerializeField] private string locationName;
    [SerializeField] private float enterDelay = 3.0f;
    [SerializeField] private float locationResetTime = 120.0f;
#pragma warning restore

    [Header("Saved in prefs")]
    public float timeToRefill;
    public bool visited = false;

    [Header("Generation")]
    public MapGenerator generatedBy;
    public int id;

    private float timeElapsedToEnter = 0.0f;
    private Image enterProgressBar;

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
        if (WorldSceneManager.Instance.CanEnterLocations)
        {
            StartCoroutine(EnterLocationRoutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StopAllCoroutines();
        timeElapsedToEnter = 0.0f;
    }

    private void Update()
    {
        enterProgressBar.fillAmount = timeElapsedToEnter / enterDelay;

        if (timeToRefill > 0)
        {
            timeToRefill -= Time.deltaTime;
        }
    }

    private IEnumerator EnterLocationRoutine()
    {
        while(true)
        {
            timeElapsedToEnter += Time.deltaTime;

            if (timeElapsedToEnter >= enterDelay)
            {
                GameplayManager.Instance.EnterLocation(locationName);

                visited = true;
                timeToRefill = locationResetTime;

                generatedBy.SaveState();
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
