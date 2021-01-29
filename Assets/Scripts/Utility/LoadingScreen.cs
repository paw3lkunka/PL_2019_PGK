using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : Singleton<LoadingScreen, AllowLazyInstancing>
{
    private AsyncOperation currentLoadingOperation;
    private bool isLoading;
    private LoadingScreenElements loadingScreenElements;

    protected override void Awake()
    {
        base.Awake();
        Hide();
    }
    

    private void Update()
    {
        if(isLoading)
        {
            loadingScreenElements.loadingBar.fillAmount = currentLoadingOperation.progress;
            if(currentLoadingOperation.isDone)
            {
                StartCoroutine(EndLoading());
            }
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        currentLoadingOperation = null;
        isLoading = false;
    }

    private IEnumerator EndLoading()
    {
        SceneObjectsManager.Instance?.InitAfterSceneLoad();
        loadingScreenElements.loadingBar.fillAmount = 1.0f;
        yield return new WaitForSeconds(0.5f);
        if(SceneObjectsManager.Instance?.initAfterSceneLoadObjects.Length > 0)
        {
            SceneObjectsManager.Instance?.initAfterSceneLoadObjects[0].GetComponentInChildren<AudioTimeline>().TimelineInit();
        }
        Hide();
    }

    public void Show(AsyncOperation loadingOperation)
    {
        if (!loadingScreenElements)
        {
            loadingScreenElements = Instantiate(ApplicationManager.Instance.PrefabDatabase.loadingScreen, transform).GetComponentInChildren<LoadingScreenElements>();
        }
        SceneObjectsManager.Instance?.DisableBeforeSceneLoad();
        gameObject.SetActive(true);
        currentLoadingOperation = loadingOperation;
        loadingScreenElements.loadingBar.fillAmount = 0.0f;
        isLoading = true;
    }
}
