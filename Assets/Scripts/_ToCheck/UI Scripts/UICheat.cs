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
        GameplayManager.Instance.Water += 0.1f;
    }

    public void Dehydration()
    {
        GameplayManager.Instance.Water -= 0.1f;
    }

    public void AddFaith()
    {
        GameplayManager.Instance.Faith += 0.1f;
    }

    public void LoseFaith()
    {
        GameplayManager.Instance.Faith -= 0.1f;
    }

    public void AddCultist()
    {
        Instantiate(ApplicationManager.prefabDatabase.cultists[0]);
    }

    #endregion
}
