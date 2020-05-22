using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopFromCanvas : MonoBehaviour
{
    public void PopCanvas()
    {
        UIOverlayManager.Instance.PopFromCanvas();
    }
}
