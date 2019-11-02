using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmTester : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private RhythmController controller;
    [SerializeField] private GameObject goodIndicator;
    [SerializeField] private GameObject greatIndicator;
    [SerializeField] private GameObject badIndicator;
#pragma warning restore

    private Beat beatStatus;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            beatStatus = controller.HitBeat();

            switch (beatStatus)
            {
                case Beat.None:
                    badIndicator.SetActive(true);
                    break;
                case Beat.Bad:
                    badIndicator.SetActive(true);
                    break;
                case Beat.Good:
                    goodIndicator.SetActive(true);
                    break;
                case Beat.Great:
                    greatIndicator.SetActive(true);
                    break;
            }
        }
        else
        {
            badIndicator.SetActive(false);
            goodIndicator.SetActive(false);
            greatIndicator.SetActive(false);
        }
    }
}
