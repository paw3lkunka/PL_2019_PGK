﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var crew = GameManager.Instance.ourCrew;
        if( crew.Count == 0 )
        {
            return;
        }
        else if( crew.Count == 1 )
        {
            transform.position = crew[0].transform.position;
        }
        else
        {
            Vector2 min = Vector2.positiveInfinity;
            Vector2 max = Vector2.negativeInfinity;

            foreach( GameObject obj in crew )
            {
                Vector2 pos = obj.transform.position;

                max.x = Mathf.Max(max.x, pos.x);
                max.y = Mathf.Max(max.y, pos.y);
                min.x = Mathf.Min(min.x, pos.x);
                min.y = Mathf.Min(min.y, pos.y);


                transform.position = min + (max - min) / 2;
                transform.position += Vector3.back * 10;
            }

        }


    }
}