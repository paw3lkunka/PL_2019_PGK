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
        // HACK: because SOMEONE made circular dependency in initializing code
        FindObjectOfType<ExitLocationUIController>()?.Setup();
        loadingBar.fillAmount = 1.0f;
        yield return new WaitForSeconds(0.5f);
        if(SceneObjectsManager.Instance?.initAfterSceneLoadObjects.Length > 0)
        {
            SceneObjectsManager.Instance?.initAfterSceneLoadObjects[0].GetComponentInChildren<AudioTimeline>().TimelineInit();
        }
        Hide();
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
