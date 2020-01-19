using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmTest : MonoBehaviour
{
    public GameObject rhythmIndicator;

    private void Start()
    {
        rhythmIndicator.SetActive(false);
    }

    private void OnEnable()
    {
        AudioTimeline.Instance.OnBeat += Indicate;
    }

    private void OnDisable()
    {
        AudioTimeline.Instance.OnBeat -= Indicate;
    }

    private void Indicate(bool isMain)
    {
        StartCoroutine(IndicateCoroutine());
    }

    private IEnumerator IndicateCoroutine()
    {
        rhythmIndicator.SetActive(true);
        yield return new WaitForEndOfFrame();
        rhythmIndicator.SetActive(false);
    }
}

