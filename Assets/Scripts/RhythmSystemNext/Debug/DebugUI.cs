using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    #region Variables

    public bool displayCountupNumber = true;
    public bool displayCombo = true;
    public bool displayRage = true;
    public bool displayTimelineState = true;
    public bool displayBarState = true;
    public bool displayBeatState = true;
    public bool displayBeatNumber = true;
    public TextMeshProUGUI textMesh;
    public Image[] images;

    public AdaptiveMusicMaster adaptiveMusic;

    private BarState barState = BarState.None;
    private BeatState beatState = BeatState.None;
    private int beatNumber = 0;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        images = GetComponentsInChildren<Image>();
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        AudioTimeline.Instance.OnBarEnd += BarDebug;
        AudioTimeline.Instance.OnBeatHit += BeatDebug;
    }

    private void OnDisable()
    {
        AudioTimeline.Instance.OnBarEnd -= BarDebug;
        AudioTimeline.Instance.OnBeatHit -= BeatDebug;
    }

    private void Update()
    {
        textMesh.text = "";
        if (displayCountupNumber)
            textMesh.text += "Countup counter: " + AudioTimeline.Instance.CountupCounter + "\n";
        if (displayCombo)
            textMesh.text += "Combo: " + RhythmMechanics.Instance.Combo + "\n";
        if (displayRage)
            textMesh.text += "Rage: " + RhythmMechanics.Instance.Rage + "\n";
        if (displayTimelineState)
            textMesh.text += "Timeline: " + AudioTimeline.Instance.TimelineState + "\n";
        if (displayBarState)
            textMesh.text += "Bar state: " + barState.ToString() + "\n";
        if (displayBeatState)
            textMesh.text += "Beat state: " + beatState.ToString() + "\n";
        if (displayBeatNumber)
            textMesh.text += "Beat number: " + beatNumber + "\n";
        if (adaptiveMusic != null && !adaptiveMusic.CurrentDrumClip.IsRealNull())
            textMesh.text += "Drum track: " + adaptiveMusic.CurrentDrumClip.name + "\n";
        //if (!lightMusicSource.IsRealNull() && !lightMusicSource.clip.IsRealNull())
        //    textMesh.text += "Drum track: " + lightMusicSource.clip.name + "\n";
        if (adaptiveMusic != null && !adaptiveMusic.CurrentHeavyMusicClip.IsRealNull())
            textMesh.text += "Heavy track: " + adaptiveMusic.CurrentHeavyMusicClip.name + "\n";
    }

    #endregion

    #region Component

    private void BarDebug(BarState barState)
    {
        this.barState = barState;
    }

    private void BeatDebug(BeatState beatState, int beatNumber)
    {
        this.beatState = beatState;
        this.beatNumber = beatNumber;
    }

    #endregion
}
