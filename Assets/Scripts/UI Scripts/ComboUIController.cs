using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComboUIController : MonoBehaviour
{
    private TextMeshProUGUI comboText;

    void Start()
    {
        comboText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable() 
    {
        RhythmMechanics.Instance.OnComboChange += UpdateCombo;
    }

    private void OnDisable() 
    {
        RhythmMechanics.Instance.OnComboChange -= UpdateCombo;
    }

    private void UpdateCombo(int value)
    {
        comboText.text = "Combo " + value;
    }
}
