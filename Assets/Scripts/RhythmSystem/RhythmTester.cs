using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmTester : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private RhythmController controller;
    [SerializeField] private Image rhythmIndicator;
    [SerializeField] private Color badColor = Color.red;
    [SerializeField] private Color goodColor = Color.yellow;
    [SerializeField] private Color greatColor = Color.green;
#pragma warning restore

    private Beat beatStatus;
    private Color transparent = new Color(0, 0, 0, 0);

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            beatStatus = controller.HitBeat();
        }
    }

}
