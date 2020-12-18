using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ComboUIController : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private Image background;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color rageColor;
    [SerializeField] private float lerpSpeed = 0.05f;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI rageText;
#pragma warning restore

    private Color currentColor;

    private void Awake()
    {
        rageText.enabled = false;
        comboText.text = "Combo 0";
        currentColor = normalColor;
    }

    private void OnEnable() 
    {
        RhythmMechanics.Instance.OnComboChange += UpdateCombo;
        RhythmMechanics.Instance.OnRageStart += RageStart;
        RhythmMechanics.Instance.OnRageStop += RageStop;
    }

    private void OnDisable() 
    {
        if(RhythmMechanics.Instance != null)
        {
            RhythmMechanics.Instance.OnComboChange -= UpdateCombo;
            RhythmMechanics.Instance.OnRageStart -= RageStart;
            RhythmMechanics.Instance.OnRageStop -= RageStop;
        }
    }

    private void UpdateCombo(int value)
    {
        if (value <= 99)
        {
            comboText.text = "Combo " + value;
        }
        else
        {
            comboText.text = "Combo ∞";
        }
    }

    private void Update()
    {
        background.color = Color.Lerp(background.color, currentColor, lerpSpeed);
    }

    private void RageStart()
    {
        currentColor = rageColor; 
        rageText.enabled = true;
    }

    private void RageStop()
    {
        currentColor = normalColor;
        rageText.enabled = false;
    }
}
