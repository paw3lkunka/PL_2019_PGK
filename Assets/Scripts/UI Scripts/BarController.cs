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
        lastCultistNumber = GameManager.Instance.cultistNumber;
        prefab.SetActive(false);
    }

    void LateUpdate()
    {
        if (lastCultistNumber < GameManager.Instance.cultistNumber)
        {
            prefab.GetComponent<InformationBar>().informationIndex = 0;
            ChangeInstance();

            instanceBeginTime = Time.time;
            lastCultistNumber = GameManager.Instance.cultistNumber;
            changed = true;
        }
        else if (lastCultistNumber > GameManager.Instance.cultistNumber && GameManager.Instance.Water < 0.2f && SceneManager.GetActiveScene().name.Equals("MainMap"))
        {
            prefab.GetComponent<InformationBar>().informationIndex = 2;
            ChangeInstance();

            instanceBeginTime = Time.time;
            lastCultistNumber = GameManager.Instance.cultistNumber;
            changed = true;
        }
        else if (lastCultistNumber > GameManager.Instance.cultistNumber && GameManager.Instance.Faith < 0.2f && SceneManager.GetActiveScene().name.Equals("MainMap"))
        {
            prefab.GetComponent<InformationBar>().informationIndex = 1;
            ChangeInstance();

            instanceBeginTime = Time.time;
            lastCultistNumber = GameManager.Instance.cultistNumber;
            changed = true;
        }
        else if (lastCultistNumber > GameManager.Instance.cultistNumber)
        {
            prefab.GetComponent<InformationBar>().informationIndex = 3;
            ChangeInstance();

            instanceBeginTime = Time.time;
            lastCultistNumber = GameManager.Instance.cultistNumber;
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
