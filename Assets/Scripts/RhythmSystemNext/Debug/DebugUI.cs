using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    public bool displayCombo = true;
    public bool displayBarState = true;
    public TextMeshProUGUI textMesh;
    public Image[] images;

    private BarState barState = BarState.None;

    void Start()
    {
        images = GetComponentsInChildren<Image>();
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        AudioTimeline.Instance.OnBarEnd += BarDebug;
    }

    private void OnDisable()
    {
        AudioTimeline.Instance.OnBarEnd -= BarDebug;
    }

    private void BarDebug(BarState barState)
    {
        this.barState = barState;
    }

    private void Update()
    {
        textMesh.text = "";
        if (displayCombo)
            textMesh.text += "Combo: " + AudioTimeline.Instance.Combo + "\n";
        if (displayBarState)
            textMesh.text += "Bar state: " + barState.ToString() + "\n";
    }

}
