using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    #region Variables



    #endregion

    #region MonoBehaviour

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AudioTimeline.Instance.BeatHit();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (AudioTimeline.Instance.TimelineState != TimelineState.Paused)
            {
                AudioTimeline.Instance.Pause();
            }
            else
            {
                AudioTimeline.Instance.Resume();
            }
        }
    }

    #endregion

    #region Component



    #endregion
}
