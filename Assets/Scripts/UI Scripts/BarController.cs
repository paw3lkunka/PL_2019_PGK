using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BarController : MonoBehaviour
{
    #region Variables

    public GameObject prefab;
    public GameObject UICanvas;

    private int lastCultistNumber;

    private List<GameObject> instances;
    private Canvas canvas;

    private float instanceBeginTime;
    private bool showLast;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        lastCultistNumber = GameManager.Instance.cultistNumber;
        instances = new List<GameObject>();
        canvas = UICanvas.GetComponent<Canvas>();
    }

    void LateUpdate()
    {
        if (lastCultistNumber < GameManager.Instance.cultistNumber)
        {
            ShiftTable();
            instances.Insert(0, Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform));
            instances[0].GetComponent<InformationBar>().informationIndex = 0;
            ChangeInstance();

            instanceBeginTime = Time.time;
            lastCultistNumber = GameManager.Instance.cultistNumber;
        }
        else if (lastCultistNumber > GameManager.Instance.cultistNumber && GameManager.Instance.Water < 0.2f)
        {
            ShiftTable();
            instances.Insert(0, Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform));
            instances[0].GetComponent<InformationBar>().informationIndex = 2;
            ChangeInstance();

            instanceBeginTime = Time.time;
            lastCultistNumber = GameManager.Instance.cultistNumber;
        }
        else if (lastCultistNumber > GameManager.Instance.cultistNumber && GameManager.Instance.Faith < 0.2f && SceneManager.GetActiveScene().name.Equals("MainMap"))
        {
            ShiftTable();
            instances.Insert(0, Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform));
            instances[0].GetComponent<InformationBar>().informationIndex = 1;
            ChangeInstance();

            instanceBeginTime = Time.time;
            lastCultistNumber = GameManager.Instance.cultistNumber;
        }
        else if (lastCultistNumber > GameManager.Instance.cultistNumber)
        {
            ShiftTable();
            instances.Insert(0, Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform));
            instances[0].GetComponent<InformationBar>().informationIndex = 3;
            ChangeInstance();

            instanceBeginTime = Time.time;
            lastCultistNumber = GameManager.Instance.cultistNumber;
        }

        if (instances.Count > 0 && instances[0].GetComponent<InformationBar>().MaxLifeTime <= (Time.time - instanceBeginTime) && !showLast)
        {
            instances[0].SetActive(false);
        }
    }

    #endregion

    #region Component

    private void ShiftTable()
    {
        if (instances.Count > 0)
        {
            if (instances[0].activeSelf)
            {
                instances[0].SetActive(false);
            }
            foreach (GameObject i in instances)
            {
                i.GetComponent<InformationBar>().barIndex += 1;
            }
        }
    }

    private void ChangeInstance()
    {
        instances[0].GetComponent<InformationBar>().barIndex = 0;
        instances[0].GetComponent<RectTransform>().anchorMin = Vector2.zero;
        instances[0].GetComponent<RectTransform>().anchorMax = Vector2.zero;
        instances[0].transform.position = Vector3.zero;
        instances[0].GetComponent<InformationBar>().UpdateBar();
        instances[0].transform.position += new Vector3(0, 50 * canvas.transform.localScale.y, 0);

    }

    public void ShowLastTen()
    {
        if (instances.Count > 0)
        {
            showLast = !showLast;
            if (showLast)
            {
                instances[0].SetActive(false);
                for (int i = 0; i < 10 && i < instances.Count; i++)
                {
                    instances[i].SetActive(true);
                    float y = 50 * canvas.transform.localScale.y + canvas.transform.localScale.y * instances[i].GetComponent<RectTransform>().rect.height * i;
                    instances[i].transform.position = new Vector3(0, y, 0);
                }
            }
            else
            {
                for (int i = 0; i < 10 && i < instances.Count; ++i)
                {
                    instances[i].SetActive(false);
                }
            }
        }
    }

    #endregion
}
