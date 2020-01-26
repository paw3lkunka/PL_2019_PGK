using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    public bool displayCombo = true;
    public bool displayBarState = true;
    public bool displayBeatState = true;
    public bool displayBeatNumber = true;
    public TextMeshProUGUI textMesh;
    public Image[] images;

    public AdaptiveMusicMaster adaptiveMusic;

    private BarState barState = BarState.None;
    private BeatState beatState = BeatState.None;
    private int beatNumber = 0;

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

    private void BarDebug(BarState barState)
    {
        this.barState = barState;
    }

    private void BeatDebug(BeatState beatState, int beatNumber)
    {
        this.beatState = beatState;
        this.beatNumber = beatNumber;
    }

    private void Update()
    {
        textMesh.text = "";
        if (displayCombo)
            textMesh.text += "Combo: " + AudioTimeline.Instance.Combo + "\n";
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

}
