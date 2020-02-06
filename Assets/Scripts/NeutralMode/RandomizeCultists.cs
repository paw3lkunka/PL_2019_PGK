using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RandomizeCultists : MonoBehaviour
{
    public int randomizedCultists;
    private System.Random rand = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        Generate();
    }

    private void Generate()
    {
        randomizedCultists = rand.Next() % 5 + 1;
        for(int i = 0; i < randomizedCultists; ++i)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
