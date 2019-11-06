using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GUI : MonoBehaviour
{

    private const int maxHeight = 128;

    [SerializeField] private RawImage faithBar;
    [SerializeField] private RawImage waterBar;
    [SerializeField] private Text faithText;
    [SerializeField] private Text waterText;
    [SerializeField] private Text cultistsText;

    public float FaithIndicator
    {
        set
        {
            faithBar.GetComponent<RectTransform>().sizeDelta = new Vector2(32, maxHeight * value);
            faithText.text = "Faith: " + (int)(value * 100) + "%";
        }
    }
    public float WaterIndicator
    {
        set
        {
            waterBar.GetComponent<RectTransform>().sizeDelta = new Vector2(32, maxHeight * value);
            waterText.text = "Water: " + (int)(value * 100) + "%";
        }
    }
    public int CrewIndicator
    {
        set
        {
            cultistsText.text = "Actual number of cult members: " + value;
        }
    }

    public void Update()
    {
        CrewIndicator = GameManager.Instance.cultistNumber;
        WaterIndicator = GameManager.Instance.Water;
        FaithIndicator = GameManager.Instance.Faith;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


}
