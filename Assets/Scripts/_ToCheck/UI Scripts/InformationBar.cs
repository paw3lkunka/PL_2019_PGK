using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InformationBar : MonoBehaviour
{
    #region Variables

    public int informationIndex { get; set; }

    public int MaxLifeTime { get; private set; }

    public static readonly string[] information =
    {
        "A new cultist arrived to your tribe!",
        "Your cultist abandoned you,\n because of low faith level",
        "Your cultist died form dehydration",
        "You cultist just died ;c"
    };

    #endregion

    #region MonoBehaviour

    #endregion

    #region Component

    public void UpdateBar()
    {
        gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = information[informationIndex];
        MaxLifeTime = 5;
    }

    #endregion    
}
