using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourcesUseIndicator : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private TextMeshProUGUI waterText;
    [SerializeField] private TextMeshProUGUI faithText;

    [SerializeField] private Color waterColor;
    [SerializeField] private Color faithColor;
    [SerializeField] private Color warningColor;
#pragma warning restore

    private float waterAmount = 0;
    private float faithAmount = 0;

    public float Water
    {
        get => waterAmount;
        set
        {
            waterAmount = value;
            waterText.text = Mathf.Approximately(faithAmount, 0) ? "0" : "-" + System.Math.Round(value, 1).ToString("0.0");
            waterText.color = value > GameplayManager.Instance.Water ? warningColor : waterColor;
        }
    }

    public float Faith
    {
        get => faithAmount;
        set
        {
            faithAmount = value;
            faithText.text = Mathf.Approximately(faithAmount, 0) ? "0" : "-" + System.Math.Round(value, 1).ToString("0.0");
            faithText.color = value > GameplayManager.Instance.Faith ? warningColor : faithColor;
        }
    }
}
