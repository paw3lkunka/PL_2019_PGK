using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : Singleton<LoadingScreen, ForbidLazyInstancing>
{
    private AsyncOperation currentLoadingOperation;
    private bool isLoading;
    [SerializeField]
    private Image loadingBar;

    protected override void Awake()
    {
        base.Awake();
        Hide();
    }
    

    private void Update()
    {
        if(isLoading)
        {
            loadingBar.fillAmount = currentLoadingOperation.progress;
            if(currentLoadingOperation.isDone)
            {
                Hide();
                SceneObjectsManager.Instance?.InitAfterSceneLoad();
            }
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        currentLoadingOperation = null;
        isLoading = false;
    }

    public void Show(AsyncOperation loadingOperation)
    {
        SceneObjectsManager.Instance?.DisableBeforeSceneLoad();
        gameObject.SetActive(true);
        currentLoadingOperation = loadingOperation;
        //currentLoadingOperation.allowSceneActivation = false;
        loadingBar.fillAmount = 0.0f;
        isLoading = true;
    }
}
