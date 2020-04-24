using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUsage : MonoBehaviour
{
    #region Variables

    public bool low = false;
    public bool high = false;
    public bool fanatic = false;

    private readonly System.Random rand = new System.Random();

    private Vector2 playerLastPosition;
    private float timeLastMemberDied = 0.0f;
    private float timeLastMemberCome = 0.0f;

    public bool isFaith = false;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        playerLastPosition = transform.position;

        if (isFaith)
        {
            ApplicationManager.Instance.LowFaithLevelStart += SetLow;
            ApplicationManager.Instance.LowFaithLevelEnd += UnsetLow;

            ApplicationManager.Instance.HighFaithLevelStart += SetHigh;
            ApplicationManager.Instance.HighFaithLevelEnd += UnsetHigh;

            ApplicationManager.Instance.FanaticStart += SetFanatic;
            ApplicationManager.Instance.FanaticEnd += UnsetFanatic;
        }
        else
        {
            ApplicationManager.Instance.LowWaterLevelStart += SetLow;
            ApplicationManager.Instance.LowWaterLevelEnd += UnsetLow;
        }
    }

    private void FixedUpdate()
    {
        if (MapSceneManager.Instance.playerPositionController.Moved)
        {
            if (isFaith)
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

            if (CrewSize > 25)
            {
                CrewSize = 25;
                while(ApplicationManager.Instance.ourCrew.Count > 25)
                {
                    Destroy(ApplicationManager.Instance.ourCrew[ApplicationManager.Instance.ourCrew.Count - 1]);
                    ApplicationManager.Instance.ourCrew.RemoveAt(ApplicationManager.Instance.ourCrew.Count - 1);
                }
            }
        }

    }

    #endregion

    #region Component

    public float UsageFactor
    {
        get
        {
            if (isFaith)
            {
                return ApplicationManager.Instance.faithUsageFactor;
            }
            else
            {
                return ApplicationManager.Instance.waterUsageFactor;
            }
        }
    }

    private float Amount
    {
        get => isFaith ? ApplicationManager.Instance.Faith : ApplicationManager.Instance.Water;
        set
        {
            if (isFaith)
            {
                ApplicationManager.Instance.Faith = value;
            }
            else
            {
                ApplicationManager.Instance.Water = value;
            }
        }
    }

    private int CrewSize
    {
        get => ApplicationManager.Instance.cultistNumber;
        set => ApplicationManager.Instance.cultistNumber = value;
    }

    private void OnLowLevel()
    {
        if ((Time.timeSinceLevelLoad - timeLastMemberDied) > (25.0f * (Amount / 0.3f)) && ApplicationManager.Instance.ourCrew.Count > 1)
        {
            CrewSize -= 1;
            int removeIndex = rand.Next() % (ApplicationManager.Instance.ourCrew.Count - 1) + 1;
            ApplicationManager.Instance.ourCrew[removeIndex].GetComponent<Cultist>().Die();
            timeLastMemberDied = Time.timeSinceLevelLoad;
        }
    }

    private void OnHighFaithLEvel()
    {
        // Powiedzieli, żeby usunąć xD
        //if ((Time.timeSinceLevelLoad - timeLastMemberCome) > 15.0f)
        //{
        //    CrewSize += 1;
        //    Instantiate(GameManager.Instance.cultistPrefab);
        //    timeLastMemberCome = Time.timeSinceLevelLoad;
        //}
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

    #endregion
}
