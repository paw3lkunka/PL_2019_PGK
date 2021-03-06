using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum PushBehaviour { Nothing, Hide, Lock };

public class UIOverlayManager : Singleton<UIOverlayManager, AllowLazyInstancing>
{
    public GameObject baseUILayer;
    public Canvas mainCanvas;

    private Stack<(GameObject, PushBehaviour)> guiObjects;

    public GameObject controlsSheetPrefab;
    public ControlsSheet ControlsSheet { get; private set; }

    #region MonoBehaviour

    protected override void Awake()
    {
        base.Awake();
        guiObjects = new Stack<(GameObject, PushBehaviour)>();

        PushToCanvas(controlsSheetPrefab);
        ControlsSheet = guiObjects.Peek().Item1.GetComponent<ControlsSheet>();

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

        var selectable = mainCanvas.GetComponentInChildren<Selectable>();
        if (selectable)
        {
            EventSystem.current.SetSelectedGameObject( selectable.gameObject );
        }
    }

#endregion

#region ManagerMethods

    public GameObject PushToCanvas(GameObject guiPrefab, PushBehaviour behaviour = PushBehaviour.Nothing)
    {
        switch (behaviour)
        {
            case PushBehaviour.Nothing:
                break;
            case PushBehaviour.Hide:
                guiObjects.Peek().Item1?.SetActive(false);
                ControlsSheet.gameObject.SetActive(true);
                break;
            case PushBehaviour.Lock:
                // guiObjects.Push((Instantiate(ApplicationManager.Instance.PrefabDatabase.lockGUI, mainCanvas.transform), PushBehaviour.Lock));
                foreach( Selectable s in guiObjects.Peek().Item1.GetComponentsInChildren<Selectable>() )
                {
                    s.interactable = false;
                }
                break;
        }
        GameObject instantiated = Instantiate(guiPrefab, mainCanvas.transform);
        guiObjects.Push((instantiated, behaviour));

        var selectable = guiObjects.Peek().Item1.GetComponentInChildren<Selectable>();
        if (selectable)
        {
            if (EventSystem.current)
            {
                EventSystem.current.SetSelectedGameObject(selectable.gameObject);
            }
        }

        return instantiated;
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
                foreach( Selectable s in guiObjects.Peek().Item1.GetComponentsInChildren<Selectable>() )
                {
                    s.interactable = true;
                }
                break;
        }

        var selectable = guiObjects.Peek().Item1.GetComponentInChildren<Selectable>();
        if (selectable)
        {
            if (EventSystem.current)
            {
                EventSystem.current.SetSelectedGameObject(selectable.gameObject);
            }
        }
    }

    #endregion
}
