using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    #region Variables

#pragma warning disable
    [SerializeField] private GameObject smokeWalk;
    [SerializeField] private GameObject smokeWater;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float startWaterLevel;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float minimumWaterLevel;
    [SerializeField] private GameObject smokeFaith;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float startFaithLevel;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float minimumFaithLevel;
#pragma warning restore

    private int minimumWalkCombo = 4;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        GameManager.Instance.Water = startWaterLevel;
        GameManager.Instance.Faith = startFaithLevel;
    }

    private void Update()
    {
        if (RhythmMechanics.Instance.Combo >= minimumWalkCombo)
        {
            Destroy(smokeWalk);
        }

        if (GameManager.Instance.Water >= minimumWaterLevel)
        {
            Destroy(smokeWater);
        }

        if (GameManager.Instance.Faith >= minimumFaithLevel)
        {
            Destroy(smokeFaith);
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            GameManager.Instance.Faith = Mathf.Max(GameManager.Instance.Faith, 0.7f);
            GameManager.Instance.Water = 1;
            SceneManager.LoadScene("MainMap");
        }
    }

    #endregion

    #region Component



    #endregion
}
