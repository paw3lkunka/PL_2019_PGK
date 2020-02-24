using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlotTrigger : MonoBehaviour
{
    #region Variables

    public bool pauseRythmFlag = false;
    [SerializeField]
    public List<string> PlotText;
    public bool cultistLeaderAloneFlag;


    public GameObject panelPrefab;
    private GameObject panelInstance;

    [SerializeField]
    private bool collisionFlag;

    #endregion

    #region MonoBehaviour

    private void LateUpdate()
    {
        // Triggers when there is only Cult Leader on Main Map scenes
        if (SceneManager.GetActiveScene().name.Equals("MainMap")
            && GameManager.Instance.ourCrew.Count == 1
            && cultistLeaderAloneFlag
            && !collisionFlag)
        {
            panelInstance = Instantiate(panelPrefab, Vector3.zero, Quaternion.identity);
            panelInstance.transform.position = new Vector3(100, 100, 0);
            panelInstance.GetComponentInChildren<PlotInformation>().PlotText = PlotText;
            collisionFlag = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (pauseRythmFlag)
        {
            AudioTimeline.Instance.Pause();
        }
        if (!collisionFlag)
        {
            panelInstance = Instantiate(panelPrefab, Vector3.zero, Quaternion.identity);
            panelInstance.transform.position = new Vector3(100, 100, 0);
            panelInstance.GetComponentInChildren<PlotInformation>().PlotText = PlotText;
            collisionFlag = true;
        }
    }

    #endregion

    #region Component



    #endregion
}
