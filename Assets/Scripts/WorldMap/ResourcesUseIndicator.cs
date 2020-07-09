using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourcesUseIndicator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI waterText;
    [SerializeField]
    private TextMeshProUGUI faithText;

    [SerializeField]
    private Color waterColor;
    [SerializeField]
    private Color faithColor;
    [SerializeField]
    private Color warningColor;

    private float waterAmmount = 0;
    private float faithAmmount = 0;

    public float Water
    {
        get => waterAmmount;
        set
        {
            waterAmmount = value;
            int intValue = Mathf.RoundToInt(waterAmmount);
            waterText.text = intValue == 0 ? "0" : "-" + intValue;
            waterText.color = waterAmmount > GameplayManager.Instance.Water ? warningColor : waterColor;
        }
    }
    public float Faith
    {
        get => faithAmmount;
        set
        {
            faithAmmount = value;
            int intValue = Mathf.RoundToInt(faithAmmount);
            faithText.text = intValue == 0 ? "0" : "-" + intValue;
            faithText.color = faithAmmount > GameplayManager.Instance.Faith ? warningColor : faithColor;
        }
    }
}
