using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExitLocationUIController : MonoBehaviour
{
    public float fadeSpeed = 0.05f;
    public TextMeshProUGUI text;
    public Image progressBarFrame;
    public Image progressBar;

    private void Update()
    {
        if(LocationManager.Instance)
        {
            if (LocationManager.Instance.ExitProgressNormalized > 0.0f)
            {
                Color tempColor;
                tempColor = text.color;
                tempColor.a = 1.0f;
                text.color = Color.Lerp(text.color, tempColor, fadeSpeed);
                tempColor = progressBarFrame.color;
                tempColor.a = 1.0f;
                progressBarFrame.color = Color.Lerp(progressBarFrame.color, tempColor, fadeSpeed);
                tempColor = progressBar.color;
                tempColor.a = 1.0f;
                progressBar.color = Color.Lerp(progressBar.color, tempColor, fadeSpeed);
            }
            else
            {
                Color tempColor;
                tempColor = text.color;
                tempColor.a = 0.0f;
                text.color = Color.Lerp(text.color, tempColor, fadeSpeed);
                tempColor = progressBarFrame.color;
                tempColor.a = 0.0f;
                progressBarFrame.color = Color.Lerp(progressBarFrame.color, tempColor, fadeSpeed);
                tempColor = progressBar.color;
                tempColor.a = 0.0f;
                progressBar.color = Color.Lerp(progressBar.color, tempColor, fadeSpeed);
            }

            progressBar.fillAmount = LocationManager.Instance.ExitProgressNormalized;
        }
    }
}
