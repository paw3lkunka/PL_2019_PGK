using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RagePanelController : MonoBehaviour
{
    private Image image;
    private TextMeshProUGUI text;

    private void Start()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        image.enabled = false;
        text.enabled = false;
    }

    private void OnEnable()
    {
        RhythmMechanics.Instance.OnRageStart += RageStart;
        RhythmMechanics.Instance.OnRageStop += RageStop;
    }

    private void OnDisable() 
    {
        RhythmMechanics.Instance.OnRageStart -= RageStart;
        RhythmMechanics.Instance.OnRageStop -= RageStop;
    }

    private void RageStart()
    {
        image.enabled = true;
        text.enabled = true;
    }

    private void RageStop()
    {
        image.enabled = false;
        text.enabled = false;
    }
}
