using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogBox : MonoBehaviour
{
    #region Variables

    public GameObject caller;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        // Set this in the middle of the screen
        gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        gameObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    #endregion

    #region Component

    public void Recruit()
    {
        Debug.LogError("Old dialog box is deprecated, and have removed functionalities.");
        //GameplayManager.Instance.cultistInfos.Add(Instantiate(ApplicationManager.Instance.PrefabDatabase.cultists[0]));
        Destroy(caller);
        Destroy(gameObject);
    }

    public void Refuse()
    {
        Destroy(gameObject);
    }

    #endregion
}
