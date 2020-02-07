using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    #region Variables

    private Image bar;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        bar = GetComponentInChildren<Image>();
    }

    #endregion

    #region Component

    public void SetBar(int hp, int maxHp)
    {
        bar.fillAmount = (float)hp / (float)maxHp;
    }

    public void HideBar()
    {
        // TODO: Make a nice fade out
    }

    public void ShowBar()
    {
        // TODO: Make a nice fade in
    }

    #endregion
}
