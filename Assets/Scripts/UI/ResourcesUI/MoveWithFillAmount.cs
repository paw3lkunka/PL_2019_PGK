using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class MoveWithFillAmount : MonoBehaviour
{
    public Vector3 maxPos;
    public Vector3 minPos;

    public Image image;

    private RectTransform rectTransform;

#if UNITY_EDITOR
    private void OnValidate()
    {
        rectTransform = GetComponent<RectTransform>();
    }
#endif

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rectTransform.anchoredPosition3D = Vector3.Lerp(minPos, maxPos, image.fillAmount);
    }
}
