using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlotTrigger : MonoBehaviour
{
    [SerializeField]
    public List<string> PlotText;
    public bool cultistLeaderAloneFlag;

    public GameObject panelPrefab;
    private GameObject panelInstance;

    [SerializeField]
    private bool collisionFlag;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!cultistLeaderAloneFlag && !collisionFlag)
        {
            panelInstance = Instantiate(panelPrefab, Vector3.zero, Quaternion.identity);
            panelInstance.transform.position = new Vector3(100, 100, 0);
            panelInstance.GetComponentInChildren<PlotInformation>().PlotText = PlotText;
            collisionFlag = true;
        }
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name.Equals("MainMap")
            && GameManager.Instance.cultistNumber == 1 
            && cultistLeaderAloneFlag
            && !collisionFlag)
        {
            panelInstance = Instantiate(panelPrefab, Vector3.zero, Quaternion.identity);
            panelInstance.transform.position = new Vector3(100, 100, 0);
            panelInstance.GetComponentInChildren<PlotInformation>().PlotText = PlotText;
            collisionFlag = true;
        }
    }
}
