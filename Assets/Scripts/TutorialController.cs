using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private GameObject smokeWalk;
    [SerializeField]
    private int minimumWalkCombo;
    [SerializeField]
    private GameObject smokeWater;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float startWaterLevel;
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float minimumWaterLevel;
    [SerializeField]
    private GameObject smokeFaith;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float startFaithLevel;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float minimumFaithLevel;

    private void Start()
    {
        GameManager.Instance.Water = startWaterLevel;
        GameManager.Instance.Faith = startFaithLevel;
    }

    private void Update()
    {
        if(RhythmController.Instance.Combo >= minimumWalkCombo)
        {
            Destroy(smokeWalk);
        }
        
        if(GameManager.Instance.Water >= minimumWaterLevel)
        {
            Destroy(smokeWater);
        }

        if(GameManager.Instance.Faith >= minimumFaithLevel)
        {
            Destroy(smokeFaith);
        }

        if (Input.GetKeyDown(KeyCode.F10))
        {
            GameManager.Instance.Faith = Mathf.Max(GameManager.Instance.Faith, 0.7f);
            GameManager.Instance.Water = 1;
            SceneManager.LoadScene(0);
        }
    }
}
