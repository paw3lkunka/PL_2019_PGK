using System.Collections;
using System.Collections.Generic;
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
    private float minimumWaterLevel;
    [SerializeField]
    private GameObject smokeFaith;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float minimumFaithLevel;

    private void Start()
    {
        GameManager.Instance.Water = 0.05f;
        GameManager.Instance.Faith = 0.5f;
    }

    private void Update()
    {
        if(RhythmController.Instance.Combo <= minimumWalkCombo)
        {
            Destroy(smokeWalk);
        }
        
        if(GameManager.Instance.Water <= minimumWaterLevel)
        {
            Destroy(smokeWater);
        }

        if(GameManager.Instance.Faith <= minimumFaithLevel)
        {
            Destroy(smokeFaith);
        }
    }
}
