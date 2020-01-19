using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKey)
        {
            AudioTimeline.Instance.BeatHit();
        }
    }
}
