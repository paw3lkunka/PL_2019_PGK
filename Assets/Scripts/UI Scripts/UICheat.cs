using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICheat : MonoBehaviour
{
    public void AddWater()
    {
        GameManager.Instance.Water += 0.1f;
    }

    public void Dehydration()
    {
        GameManager.Instance.Water -= 0.1f;
    }

    public void AddFaith()
    {
        GameManager.Instance.Faith += 0.1f;
    }

    public void LoseFaith()
    {
        GameManager.Instance.Faith -= 0.1f;
    }

    public void AddCultist()
    {
        GameManager.Instance.cultistNumber += 1;
        Instantiate(GameManager.Instance.cultistPrefab);
    }

    public void KillCultist()
    {
        GameManager.Instance.cultistNumber -= 1;
    }
}
