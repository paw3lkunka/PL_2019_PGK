using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image bar;

    private void Awake()
    {
        bar = GetComponentInChildren<Image>();
    }

    public void SetBar(int hp, int maxHp)
    {
        bar.fillAmount = (float)hp / maxHp;
    }

    public void HideBar()
    {
        // TODO: Make a nice fade out
    }

    public void ShowBar()
    {
        // TODO: Make a nice fade in
    }
}
