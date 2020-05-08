using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayUIBaseController : MonoBehaviour
{
    public void Back()
    {
        UIOverlayManager.Instance.PopFromCanvas();
    }
}
