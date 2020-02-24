using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Shrine : MonoBehaviour
{
    #region Variables

    public GameObject hiddenEnemies;
    bool visited = false;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        if (GameManager.Instance.ShrinesVisited.Contains(GameManager.Instance.currentLocation))
        {
            visited = true;
        }
        else
        {
            hiddenEnemies.SetActive(false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!visited && collision.gameObject.GetComponent<Bullet>() == null)
        {
            hiddenEnemies.SetActive(true);
            GameManager.Instance.ShrinesVisited.Add(GameManager.Instance.currentLocation);
            GameManager.Instance.Faith += 0.2f;
            visited = true;
        }
    }

    #endregion

    #region Component



    #endregion
}
