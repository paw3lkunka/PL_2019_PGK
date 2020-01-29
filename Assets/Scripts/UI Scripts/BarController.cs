using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarController : MonoBehaviour
{
    public GameObject prefab;
    public GameObject UICanvas;

    private int lastCultistNumber;

    private List<GameObject> instances;
    private Canvas canvas;

    private float instanceBeginTime;
    private bool showLast;

    // Start is called before the first frame update
    void Start()
    {
        lastCultistNumber = GameManager.Instance.cultistNumber;
        instances = new List<GameObject>();
        canvas = UICanvas.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(lastCultistNumber < GameManager.Instance.cultistNumber)
        {
            shiftTable();
            instances.Insert(0, Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity, canvas.transform));
            instances[0].GetComponent<InformationBar>().informationIndex = 0;
            changeInstance();
            
            instanceBeginTime = Time.time;
            lastCultistNumber = GameManager.Instance.cultistNumber;
        }
        else if(lastCultistNumber > GameManager.Instance.cultistNumber && GameManager.Instance.Water < 0.2f)
        {
            shiftTable();
            instances.Insert(0, Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity, canvas.transform));
            instances[0].GetComponent<InformationBar>().informationIndex = 2;
            changeInstance();
            
            instanceBeginTime = Time.time;
            lastCultistNumber = GameManager.Instance.cultistNumber;
        }
        else if(lastCultistNumber > GameManager.Instance.cultistNumber && GameManager.Instance.Faith < 0.2f)
        {
            shiftTable();
            instances.Insert(0, Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity, canvas.transform));
            instances[0].GetComponent<InformationBar>().informationIndex = 1;
            changeInstance();
            
            instanceBeginTime = Time.time;
            lastCultistNumber = GameManager.Instance.cultistNumber;
        }
        else if(lastCultistNumber > GameManager.Instance.cultistNumber)
        {
            shiftTable();
            instances.Insert(0, Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity, canvas.transform));
            instances[0].GetComponent<InformationBar>().informationIndex = 3;
            changeInstance();
            
            instanceBeginTime = Time.time;
            lastCultistNumber = GameManager.Instance.cultistNumber;
        }

        if(instances.Count > 0 && instances[0].GetComponent<InformationBar>().MaxLifeTime <= (Time.time - instanceBeginTime) && !showLast)
        {
            instances[0].SetActive(false);
        }
    }

    private void shiftTable()
    { 
        if(instances.Count > 0)
        {
            if(instances[0].activeSelf)
            {
                instances[0].SetActive(false);
            }
            foreach(GameObject i in instances)
            {
                i.GetComponent<InformationBar>().barIndex += 1;
            }
        }
    }

    private void changeInstance()
    {
        instances[0].GetComponent<InformationBar>().barIndex = 0;
        instances[0].GetComponent<InformationBar>().UpdateBar();
        instances[0].transform.position += new Vector3(0, 50 * canvas.transform.localScale.y, 0);
    }

    public void ShowLastTen()
    {
        if(instances.Count > 0)
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
}
