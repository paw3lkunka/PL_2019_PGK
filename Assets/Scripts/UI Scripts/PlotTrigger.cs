using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotTrigger : MonoBehaviour
{
    [SerializeField]
    public List<string> PlotText;

    public GameObject panelPrefab;
    private GameObject panelInstance;

    private bool collisionFlag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collisionFlag)
        {
            panelInstance = Instantiate(panelPrefab, Vector3.zero, Quaternion.identity);
            panelInstance.transform.position = new Vector3(100, 100, 0);
            panelInstance.GetComponentInChildren<PlotInformation>().PlotText = PlotText;
            collisionFlag = true;
        }
    }
}
