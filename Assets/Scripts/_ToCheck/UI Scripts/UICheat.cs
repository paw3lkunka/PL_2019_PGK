using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICheat : MonoBehaviour
{
    #region Variables



    #endregion

    #region MonoBehaviour

    private void Start() 
    {
        if (!ApplicationManager.Instance.enableCheats)
        {
            gameObject.SetActive(false);
        }
    }

    #endregion

    #region Component

    public void AddWater()
    {
        GameplayManager.Instance.Water += 1.0f;
    }

    public void Dehydration()
    {
        GameplayManager.Instance.Water -= 1.0f;
    }

    public void AddFaith()
    {
        GameplayManager.Instance.Faith += 1.0f;
    }

    public void LoseFaith()
    {
        GameplayManager.Instance.Faith -= 1.0f;
    }

    public void AddCultist()
    {
        Instantiate(ApplicationManager.Instance.PrefabDatabase.cultists[0]);
    }

    #endregion
}
