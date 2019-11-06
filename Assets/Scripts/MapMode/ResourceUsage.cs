﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUsage : MonoBehaviour
{
    public float usageFactor = 0.0002f;
    public bool isFaith = false;

    private Vector2 playerLastPosition;
    private float timeLastMemberDied = 0.0f;
    private float timeLastMemberCome = 0.0f;

    private float Amount
    {
        get => isFaith ? GameManager.Instance.Faith : GameManager.Instance.Water;
        set
        {
            if (isFaith)
            {
                GameManager.Instance.Faith = value;
            }
            else
            {
                GameManager.Instance.Water = value;
            }
        }
    }

    private int crewSize
    {
        get => GameManager.Instance.cultistNumber;
        set => GameManager.Instance.cultistNumber = value;
    }


    // Start is called before the first frame update
    void Start()
    {
        playerLastPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            Amount = 1.0f;
        }
        if(playerLastPosition.x != transform.position.x || playerLastPosition.y != transform.position.y)
        {
            if(isFaith)
            {
                Amount -= usageFactor * (crewSize > 5.0f ? (crewSize / 5) : 1.0f);
                //"Faith strenghtening"
                Amount += usageFactor * (crewSize > 7.0f ? (crewSize / 7) : 0.0f);
            }
            else
            {
                Amount -= usageFactor * (crewSize > 2.0f ? (crewSize / 2) : 1.0f);
            }
            

            if( Amount < 0.25f && 
                (Time.time - timeLastMemberDied) > (25.0f * (Amount / 0.3f)) )
            {
                crewSize -= 1;
                timeLastMemberDied = Time.time;
            }

            if( isFaith && Amount > 0.9f 
            && (Time.time - timeLastMemberDied) > ( 25.0f * (1.0 - (Amount / 0.9f - 1.0f)) ) )
            {
                crewSize -= 1;
                timeLastMemberDied = Time.time;
            }

            if( isFaith && Amount > 0.7f
            && (Time.time - timeLastMemberCome) >  15.0f )
            {
                crewSize += 1;
                timeLastMemberCome = Time.time;
            }
            
            playerLastPosition = transform.position;

            if(crewSize > 25) crewSize = 25;
        }
    }
}
