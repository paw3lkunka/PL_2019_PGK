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
    [SerializeField] private Image waterTop;
    [SerializeField] private Image warningLight;
    [SerializeField] private AudioSource waterAlarm;
    [SerializeField] private TextMeshProUGUI waterAmount;
    [SerializeField] private TextMeshProUGUI waterMax;
    [Header("Faith")]
    [SerializeField] private Image faithBar;
    [SerializeField] private Color faithBarNormColor;
    [SerializeField] private Color faithBarOvrfColor;
    [SerializeField] private TextMeshProUGUI faithAmount;
    [Header("Health")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthAmount;
    [Header("Cultists")]
    [SerializeField] private TextMeshProUGUI cultistsAmount;
#pragma warning restore

    private bool warningEnabled = false;

    private void Start()
    {
        warningLight.gameObject.SetActive(false);
    }

    void Update()
    {
        waterBar.fillAmount = GameplayManager.Instance.Water.Normalized;
        waterTop.enabled = GameplayManager.Instance.Water.Normalized > 0;
        if (GameplayManager.Instance.Water.Normalized < GameplayManager.Instance.lowWaterLevel && !warningEnabled)
        {
            warningLight.gameObject.SetActive(true);
            warningEnabled = true;
            waterAlarm.Play();
        }
        else if (GameplayManager.Instance.Water.Normalized >= GameplayManager.Instance.lowWaterLevel && warningEnabled)
        {
            warningLight.gameObject.SetActive(false);
            warningEnabled = false;
        }
        waterAmount.text = System.Math.Round(GameplayManager.Instance.Water, 1).ToString("000.0").Remove(3, 1).Insert(3, "<color=black>").Insert(0, "<mspace=8.0>");
        waterMax.text = System.Math.Round(GameplayManager.Instance.Water.Max, 1).ToString("000.0").Remove(3, 1).Insert(3, "<color=black>").Insert(0, "<mspace=8.0>");

        var faith = GameplayManager.Instance.Faith.Normalized;
        faithBar.fillAmount = faith;
        faithBar.color = GameplayManager.Instance.overfaith ? faithBarOvrfColor : faithBarNormColor;
        faithAmount.text = System.Math.Round(GameplayManager.Instance.Faith, 1).ToString("0.0");
        
        healthBar.fillAmount = GameplayManager.Instance.Health / GameplayManager.Instance.MaxHealth;
        healthAmount.text = System.Math.Round(GameplayManager.Instance.Health, 1).ToString("0.0");

        cultistsAmount.text = GameplayManager.Instance.cultistInfos.Count.ToString("00");
    }
}
