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

    public int generationID;

    private float timeElapsedToEnter = 0.0f;
    private Image enterProgressBar;

    private void Awake()
    {
        enterProgressBar = GetComponentInChildren<Image>();
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(EnterLocationRoutine());
    }

    private void OnTriggerExit(Collider other)
    {
        StopAllCoroutines();
        timeElapsedToEnter = 0.0f;
    }

    private void Update()
    {
        enterProgressBar.fillAmount = timeElapsedToEnter / enterDelay;
    }

    private IEnumerator EnterLocationRoutine()
    {
        while(true)
        {
            timeElapsedToEnter += Time.deltaTime;

            if (timeElapsedToEnter >= enterDelay)
            {
                ApplicationManager.Instance.EnterLocation(locationName);
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
