using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform bar;

    private void Awake()
    {
        bar = this.transform.Find("Bar");
    }

    public void SetBar(int hp, int maxHp)
    {
        var normalizedHp = (float)hp / (float)maxHp;
        bar.localScale = new Vector3(normalizedHp, 1.0f);
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
