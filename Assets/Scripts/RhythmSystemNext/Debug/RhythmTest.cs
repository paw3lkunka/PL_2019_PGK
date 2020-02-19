using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmTest : MonoBehaviour
{
    #region Variables

    public Image rhythmIndicator;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        rhythmIndicator = GameObject.Find("RhythmIndicator").GetComponent<Image>();
        if (rhythmIndicator.IsRealNull())
        {
            rhythmIndicator = FindObjectOfType<Image>();
        }
        rhythmIndicator.enabled = false;
    }

    private void OnEnable()
    {
        AudioTimeline.Instance.OnBeat += Indicate;
        AudioTimeline.Instance.OnBeatHit += VisualizeBeatHit;
    }

    private void OnDisable()
    {
        AudioTimeline.Instance.OnBeat -= Indicate;
        AudioTimeline.Instance.OnBeatHit -= VisualizeBeatHit;
    }

    #endregion

    #region Component

    private void VisualizeBeatHit(BeatState state, int beatNumber)
    {
        switch (state)
        {
            case BeatState.None:
                rhythmIndicator.color = Color.black;
                break;
            case BeatState.Bad:
                rhythmIndicator.color = Color.red;
                break;
            case BeatState.Good:
                rhythmIndicator.color = Color.cyan;
                break;
            case BeatState.Great:
                rhythmIndicator.color = Color.green;
                break;
            case BeatState.Perfect:
                rhythmIndicator.color = Color.magenta;
                break;
        }
        Indicate(false);
    }

    private void Indicate(bool isMain)
    {
        StartCoroutine(IndicateCoroutine());
    }

    private IEnumerator IndicateCoroutine()
    {
        rhythmIndicator.enabled = true;
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        rhythmIndicator.enabled = false;
        rhythmIndicator.color = Color.white;
    }

    #endregion
}

