using UnityEngine;
using UnityEngine.SceneManagement;

public class BarController : MonoBehaviour
{
    #region Variables

    public GameObject prefab;
    public GameObject UICanvas;

    private int lastCultistNumber;

    private float instanceBeginTime;
    private bool changed = false;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        lastCultistNumber = GameplayManager.Instance.cultistInfos.Count;
        prefab.SetActive(false);
    }

    void LateUpdate()
    {
        if (lastCultistNumber < GameplayManager.Instance.cultistInfos.Count)
        {
            prefab.GetComponent<InformationBar>().informationIndex = 0;
            ChangeInstance();

            instanceBeginTime = Time.time;
            lastCultistNumber = GameplayManager.Instance.cultistInfos.Count;
            changed = true;
        }
        else if (lastCultistNumber > GameplayManager.Instance.cultistInfos.Count 
                && GameplayManager.Instance.Water < 0.2f 
                && SceneManager.GetActiveScene().name.Equals(ApplicationManager.Instance.worldMapScene))
        {
            prefab.GetComponent<InformationBar>().informationIndex = 2;
            ChangeInstance();

            instanceBeginTime = Time.time;
            lastCultistNumber = GameplayManager.Instance.cultistInfos.Count;
            changed = true;
        }
        else if (lastCultistNumber > GameplayManager.Instance.cultistInfos.Count 
                && GameplayManager.Instance.Faith < 0.2f 
                && SceneManager.GetActiveScene().name.Equals(ApplicationManager.Instance.worldMapScene))
        {
            prefab.GetComponent<InformationBar>().informationIndex = 1;
            ChangeInstance();

            instanceBeginTime = Time.time;
            lastCultistNumber = GameplayManager.Instance.cultistInfos.Count;
            changed = true;
        }
        else if (lastCultistNumber > GameplayManager.Instance.cultistInfos.Count)
        {
            prefab.GetComponent<InformationBar>().informationIndex = 3;
            ChangeInstance();

            instanceBeginTime = Time.time;
            lastCultistNumber = GameplayManager.Instance.cultistInfos.Count;
            changed = true;
        }

        if (changed && prefab.GetComponent<InformationBar>().MaxLifeTime <= (Time.time - instanceBeginTime))
        {
            prefab.SetActive(false);
        }

    }

    #endregion

    #region Component


    private void ChangeInstance()
    {
        prefab.SetActive(true);
        prefab.GetComponent<InformationBar>().UpdateBar();
    }

    #endregion
}
