using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Damageable damageable;
    private Image bar;

    private void Awake()
    {
        damageable = GetComponentInParent<Damageable>();
        bar = GetComponentInChildren<Image>();
    }

    private void Update()
    {
        bar.fillAmount = damageable.health.Normalized;
    }
}
