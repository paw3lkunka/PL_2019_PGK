using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RandomizeCultists : MonoBehaviour
{
    #region Variables

    public int randomizedCultists;
    private System.Random rand = new System.Random();

    #endregion

    #region MonoBehaviour

    void Start()
    {
        for (int i = 0; i < 5; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        Generate();
    }

    #endregion

    #region Component

    private void Generate()
    {
        randomizedCultists = rand.Next() % 5 + 1;
        for (int i = 0; i < randomizedCultists; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    #endregion
}
