using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GUI : MonoBehaviour
{
    #region Variables

#pragma warning disable
    [SerializeField] private Slider faithBar;
    [SerializeField] private Slider waterBar;
    [SerializeField] private TextMeshProUGUI faithText;
    [SerializeField] private TextMeshProUGUI waterText;
    [SerializeField] private TextMeshProUGUI cultistsText;
#pragma warning restore

    public bool CheatSheet;
    public bool isMouseOver;

    private Image faithImage;
    private Image waterImage;

    private readonly Color32 faithMedium = new Color32(255, 133, 0, 255); // orange
    private readonly Color32 faithOverflow = new Color32(255, 0, 0, 255); // red
    private readonly Color32 faithLow = new Color32(255, 200, 138, 255); // light orange

    private readonly Color32 waterGood = new Color32(0, 40, 255, 255); // blue
    private readonly Color32 waterBad = new Color32(108, 130, 255, 255); // light blue

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        faithImage = faithBar.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
        waterImage = waterBar.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
        if (!CheatSheet)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        CrewIndicator = GameManager.Instance.cultistNumber;
        WaterIndicator = GameManager.Instance.Water;
        FaithIndicator = GameManager.Instance.Faith;

        if (FaithIndicator >= 0.9f)
        {
            faithImage.color = faithOverflow;
        }
        else if (FaithIndicator <= 0.2f)
        {
            faithImage.color = faithLow;
        }
        else
        {
            faithImage.color = faithMedium;
        }

        if (WaterIndicator <= 0.2f)
        {
            waterImage.color = waterBad;
        }
        else
        {
            waterImage.color = waterGood;
        }
    }

    #endregion

    #region Component

    public float FaithIndicator
    {
        set
        {
            faithBar.value = value;
            faithText.text = "Faith: " + (int)(value * 100) + "%";
        }
        get => faithBar.value;
    }

    public float WaterIndicator
    {
        set
        {
            waterBar.value = value;
            waterText.text = "Water: " + (int)(value * 100) + "%";
        }
        get => waterBar.value;
    }

    public int CrewIndicator
    {
        set
        {
            cultistsText.text = "Cultists: " + value;
        }
    }

    public Canvas UICanvas
    {
        get => GetComponent<Canvas>();
    }

    public void PointerEnter()
    {
        isMouseOver = true;
    }

    public void PointerExit()
    {
        isMouseOver = false;
    }

    public void Initialize()
    {
        UICanvas.worldCamera = FindObjectOfType<Camera>();
        UICanvas.sortingLayerName = "UI";
    }

    #endregion
}
