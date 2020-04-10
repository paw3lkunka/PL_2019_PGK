using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExitDialogBoxController : MonoBehaviour
{
    #region Variables

    public GameObject dialogBoxPrefab;

    private GameObject boxInstance;

    #endregion

    #region MonoBehaviour


    #endregion

    #region Component

    public void DialogCaller()
    {
        boxInstance = Instantiate(dialogBoxPrefab, gameObject.transform.GetComponentInParent<Canvas>().transform);
        // Set this in the middle of the screen
        boxInstance.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        boxInstance.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        boxInstance.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        boxInstance.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(YesOption);
        boxInstance.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(NoOption);
        boxInstance.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Are you sure that you want to return to main menu? You will loose all your progress.";
    }

    public void YesOption()
    {
        GameManager.Instance.BackToMenu();
        Destroy(boxInstance);
    }

    public void NoOption()
    {
        Destroy(boxInstance);
    }

    #endregion
}
