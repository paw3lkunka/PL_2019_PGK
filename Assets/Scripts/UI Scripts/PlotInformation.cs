using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlotInformation : MonoBehaviour
{
    #region Variables

    public List<string> PlotText;
    private int infoIndex;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        infoIndex = -1;
        NextText();
    }

    #endregion

    #region Component

    public void NextText()
    {
        infoIndex += 1;
        if (infoIndex < PlotText.Count)
        {
            transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = PlotText[infoIndex];
        }

        if (infoIndex < (PlotText.Count - 1))
        {
            GetComponentInChildren<Button>().transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Next information";
        }
        else if (infoIndex == (PlotText.Count - 1))
        {
            GetComponentInChildren<Button>().transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "OK";
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion
}
