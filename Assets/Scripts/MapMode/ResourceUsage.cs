﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUsage : MonoBehaviour
{

    public bool low = false, high = false, fanatic = false;

    /*
    //Ja to tu tylko zostawię
    private void OnValidate()
    {
        throw new System.Exception("xD");
    }
    */

    public float UsageFactor
    {
        get
        {
            if(isFaith)
            {
                return GameManager.Instance.FaithUsageFactor;
            }
            else
            {
                return GameManager.Instance.WaterUsageFactor;
            }
        }
    }
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

    private int CrewSize
    {
        get => GameManager.Instance.cultistNumber;
        set => GameManager.Instance.cultistNumber = value;
    }


    // Start is called before the first frame update
    void Start()
    {
        playerLastPosition = transform.position;

        if(isFaith)
        {
            GameManager.Instance.LowFaithLevelStart += SetLow;
            GameManager.Instance.LowFaithLevelEnd += UnsetLow;
            
            GameManager.Instance.HighFaithLevelStart += SetHigh;
            GameManager.Instance.HighFaithLevelEnd += UnsetHigh;

            GameManager.Instance.FanaticStart += SetFanatic;
            GameManager.Instance.FanaticEnd += UnsetFanatic;
        }
        else
        {
            GameManager.Instance.LowWaterLevelStart += SetLow;
            GameManager.Instance.LowWaterLevelEnd += UnsetLow;
        }
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
                Amount -= UsageFactor * (CrewSize > 7.0f ? (CrewSize / 7) : 1.0f);
                //"Faith strenghtening"
                Amount += UsageFactor * (CrewSize > 9.0f ? (CrewSize / 9) : 0.0f);
            }
            else
            {
                Amount -= UsageFactor * (CrewSize > 5.0f ? (CrewSize / 5) : 1.0f);
            }

            if (low)
                OnLowLevel();

            if (isFaith)
            {
                if (high)
                    OnHighFaithLEvel();

                if (fanatic)
                    OnFanatic();
            }

            playerLastPosition = transform.position;

            if(CrewSize > 25) CrewSize = 25;
        }

    }

    private void OnLowLevel()
    {
        if ( (Time.timeSinceLevelLoad - timeLastMemberDied) > (25.0f * (Amount / 0.3f) ) )
        {
            CrewSize -= 1;
            timeLastMemberDied = Time.timeSinceLevelLoad;
        }
    }

    private void OnHighFaithLEvel()
    {
        if ( (Time.timeSinceLevelLoad - timeLastMemberCome) > 15.0f )
        {
            CrewSize += 1;
            Instantiate(GameManager.Instance.cultistPrefab);
            timeLastMemberCome = Time.timeSinceLevelLoad;
        }
    }
    private void OnFanatic()
    {
        //Nie wiem, czy przy utraci kontroli jest to jesczcze potrzebne
        /*
        if ( (Time.timeSinceLevelLoad - timeLastMemberDied) > (25.0f * (1.0 - (Amount / 0.9f - 1.0f))) )
        {
            CrewSize -= 1;
            timeLastMemberDied = Time.timeSinceLevelLoad;
        }
        */
    }

    private void SetLow() => low = true;
    private void SetHigh() => high = true;
    private void SetFanatic() => fanatic = true;
    private void UnsetLow() => low = false;
    private void UnsetHigh() => high = false;
    private void UnsetFanatic() => fanatic = false;
}
