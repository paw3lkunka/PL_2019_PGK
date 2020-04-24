using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOverlayManager : Singleton<UIOverlayManager>
{
    public GameObject baseUILayer;
    public Canvas mainCanvas;

    private Stack<GameObject> guiObjects;

#region MonoBehaviour
    
    private void Awake()
    {
        guiObjects = new Stack<GameObject>();
    }

    private void Start() 
    {
        if (mainCanvas == null)
        {
            GameObject canvasObject = Instantiate(new GameObject());
            canvasObject.name = "Main Canvas";
            mainCanvas = canvasObject.AddComponent<Canvas>();
            mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
        if (baseUILayer)
        {
            guiObjects.Push(baseUILayer);
        }
    }

#endregion

#region ManagerMethods

    public void PushToCanvas(GameObject guiPrefab, bool hideLast = false)
    {
        guiObjects.Peek().SetActive(!hideLast);
        guiObjects.Push(Instantiate(guiPrefab, mainCanvas.transform));
    }

    public void PopFromCanvas()
    {
        Destroy(guiObjects.Pop());
        guiObjects.Peek().SetActive(true);
    }

#endregion
}
