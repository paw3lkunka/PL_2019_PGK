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

    private float waterAmmount = 0;
    private float faithAmmount = 0;

    public float Water
    {
        get => waterAmmount;
        set
        {
            waterAmmount = value;
            waterText.text = Mathf.Approximately(faithAmmount, 0) ? "0" : "-" + System.Math.Round(value, 1).ToString("0.0");
            waterText.color = value > GameplayManager.Instance.Water ? warningColor : waterColor;
        }
    }

    public float Faith
    {
        get => faithAmmount;
        set
        {
            faithAmmount = value;
            faithText.text = Mathf.Approximately(faithAmmount, 0) ? "0" : "-" + System.Math.Round(value, 1).ToString("0.0");
            faithText.color = value > GameplayManager.Instance.Faith ? warningColor : faithColor;
        }
    }
}
