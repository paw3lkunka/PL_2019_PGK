using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOverlayManager : Singleton<UIOverlayManager>
{
    private Canvas mainCanvas;
    private Stack<GameObject> guiObjects;

#region MonoBehaviour
    
    private void Start() 
    {
        mainCanvas = Instantiate(new Canvas());
        mainCanvas.name = "Main Canvas";
    }

#endregion

#region ManagerMethods

    public void PushToCanvas(GameObject guiPrefab)
    {
        guiObjects.Push(Instantiate(guiPrefab, mainCanvas.transform));
    }

    public void PopFromCanvas()
    {
        Destroy(guiObjects.Pop());
    }

#endregion
}
