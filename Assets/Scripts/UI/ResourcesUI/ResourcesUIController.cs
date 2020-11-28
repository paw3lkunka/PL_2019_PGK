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
    [SerializeField] private TextMeshProUGUI waterMax;
    [Header("Faith")]
    [SerializeField] private Image faithBar;
    [SerializeField] private TextMeshProUGUI faithAmount;
    [Header("Health")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthAmount;
    [Header("Cultists")]
    [SerializeField] private TextMeshProUGUI cultistsAmount;
#pragma warning restore

    void Update()
    {
        waterBar.fillAmount = GameplayManager.Instance.Water.Normalized;
        waterAmount.text = System.Math.Round(GameplayManager.Instance.Water, 1).ToString("000.0").Remove(3, 1).Insert(3, "<color=black>").Insert(0, "<mspace=8.0>");
        waterMax.text = System.Math.Round(GameplayManager.Instance.Water.Max, 1).ToString("000.0").Remove(3, 1).Insert(3, "<color=black>").Insert(0, "<mspace=8.0>");

        faithBar.fillAmount = GameplayManager.Instance.Faith.Normalized;
        faithAmount.text = System.Math.Round(GameplayManager.Instance.Faith, 1).ToString("0.0");
        
        healthBar.fillAmount = GameplayManager.Instance.Health / GameplayManager.Instance.MaxHealth;
        healthAmount.text = System.Math.Round(GameplayManager.Instance.Health, 1).ToString("0.0");

        cultistsAmount.text = GameplayManager.Instance.cultistInfos.Count.ToString("00");
    }
}
