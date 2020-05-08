using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesUIController : MonoBehaviour
{
#pragma warning disable
    [Header("Water")]
    [SerializeField] private Image waterBar;
    [SerializeField] private TextMeshProUGUI waterAmount;
    [Header("Faith")]
    [SerializeField] private Image faithBar;
    [SerializeField] private TextMeshProUGUI faithAmount;
#pragma warning restore

    void Update()
    {
        // TODO: Add support for unnormalized water amount
        waterBar.fillAmount = GameplayManager.Instance.Water;
        waterAmount.text = (GameplayManager.Instance.Water * 100).ToString();
        // TODO: Add support for unnormalized faith amount
        faithBar.fillAmount = GameplayManager.Instance.Faith;
        faithAmount.text = (GameplayManager.Instance.Faith * 100).ToString();
    }
}
