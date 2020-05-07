using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum PushBehaviour { Nothing, Hide, Lock };

public class UIOverlayManager : Singleton<UIOverlayManager>
{
    public GameObject baseUILayer;
    public Canvas mainCanvas;

    private Stack<(GameObject, PushBehaviour)> guiObjects;

#region MonoBehaviour
    
    private void Awake()
    {
        guiObjects = new Stack<(GameObject, PushBehaviour)>();
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
            guiObjects.Push((baseUILayer, PushBehaviour.Nothing));
        }

        EventSystem.current.SetSelectedGameObject( mainCanvas.GetComponentInChildren<Selectable>().gameObject );
    }

#endregion

#region ManagerMethods

    public void PushToCanvas(GameObject guiPrefab, PushBehaviour behaviour = PushBehaviour.Nothing)
    {
        switch (behaviour)
        {
            case PushBehaviour.Nothing:
                break;
            case PushBehaviour.Hide:
                guiObjects.Peek().Item1?.SetActive(false);
                break;
            case PushBehaviour.Lock:
                guiObjects.Push((Instantiate(ApplicationManager.prefabDatabase.lockGUI, mainCanvas.transform), PushBehaviour.Lock));
                break;
        }
        guiObjects.Push((Instantiate(guiPrefab, mainCanvas.transform), behaviour));

        EventSystem.current.SetSelectedGameObject( guiObjects.Peek().Item1.GetComponentInChildren<Selectable>().gameObject );
    }

    public void PopFromCanvas()
    {
        switch (guiObjects.Peek().Item2)
        {
            case PushBehaviour.Nothing:
                Destroy(guiObjects.Pop().Item1);
                break;
            case PushBehaviour.Hide:
                Destroy(guiObjects.Pop().Item1);
                guiObjects.Peek().Item1?.SetActive(true);
                break;
            case PushBehaviour.Lock:
                Destroy(guiObjects.Pop().Item1);
                Destroy(guiObjects.Pop().Item1);
                break;
        }

        EventSystem.current.SetSelectedGameObject( guiObjects.Peek().Item1.GetComponentInChildren<Selectable>().gameObject );
    }

    #endregion
}
