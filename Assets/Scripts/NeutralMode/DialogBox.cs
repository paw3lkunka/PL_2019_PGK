using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogBox : MonoBehaviour
{
    public GameObject caller;

    private void Awake()
    {
        // Set this in the middle of the screen
        gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        gameObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }
    public void Recruit()
    {
        Instantiate(GameManager.Instance.cultistPrefab);
        GameManager.Instance.cultistNumber += 1;
        Destroy(caller);
        Destroy(gameObject);
    }

    public void Refuse()
    {
        Destroy(gameObject);
    }
}
