using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InformationBar
{
    private GameObject prefab;

    public int MaxLifeTime { get; private set; }

    public int LifeTime;

    InformationBar(GameObject uiPrefab, string information)
    {
        prefab = uiPrefab;
        prefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = information;
        MaxLifeTime = 8;
    }
}
