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
            GameplayManager.Instance.LowFaithLevelStart += SetLow;
            GameplayManager.Instance.LowFaithLevelEnd += UnsetLow;

            GameplayManager.Instance.HighFaithLevelStart += SetHigh;
            GameplayManager.Instance.HighFaithLevelEnd += UnsetHigh;

            GameplayManager.Instance.FanaticStart += SetFanatic;
            GameplayManager.Instance.FanaticEnd += UnsetFanatic;
        }
        else
        {
            GameplayManager.Instance.LowWaterLevelStart += SetLow;
            GameplayManager.Instance.LowWaterLevelEnd += UnsetLow;
        }
    }

    private void FixedUpdate()
    {
        if (MapSceneManager.Instance.playerPositionController.Moved)
        {
            int crewSize = GameplayManager.Instance.ourCrew.Count;
            if (isFaith)
            {
                Amount -= UsageFactor * (crewSize > 7 ? (crewSize / 7.0f) : 1.0f);
                //"Faith strenghtening"
                Amount += UsageFactor * (crewSize > 9 ? (crewSize / 9.0f) : 0.0f);
            }
            else
            {
                Amount -= UsageFactor * (crewSize > 5 ? (crewSize / 5.0f) : 1.0f);
            }

            if (low)
            {
                OnLowLevel();
            }

            if (isFaith)
            {
                if (high)
                {
                    OnHighFaithLevel();
                }

                if (fanatic)
                {
                    OnFanatic();
                }
            }

            playerLastPosition = transform.position;

            if (crewSize > 25)
            {
                while(GameplayManager.Instance.ourCrew.Count > 25)
                {
                    Destroy(GameplayManager.Instance.ourCrew[GameplayManager.Instance.ourCrew.Count - 1]);
                    GameplayManager.Instance.ourCrew.RemoveAt(GameplayManager.Instance.ourCrew.Count - 1);
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
                return GameplayManager.Instance.faithUsageFactor;
            }
            else
            {
                return GameplayManager.Instance.waterUsageFactor;
            }
        }
    }

    private float Amount
    {
        get => isFaith ? GameplayManager.Instance.Faith : GameplayManager.Instance.Water;
        set
        {
            if (isFaith)
            {
                GameplayManager.Instance.Faith = value;
            }
            else
            {
                GameplayManager.Instance.Water = value;
            }
        }
    }

    private void OnLowLevel()
    {
        if ((Time.timeSinceLevelLoad - timeLastMemberDied) > (25.0f * (Amount / 0.3f)) && GameplayManager.Instance.ourCrew.Count > 1)
        {
            int removeIndex = rand.Next() % (GameplayManager.Instance.ourCrew.Count - 1) + 1;
            GameplayManager.Instance.ourCrew[removeIndex].GetComponent<Cultist3d>().Die();
            timeLastMemberDied = Time.timeSinceLevelLoad;
        }
    }

    private void OnHighFaithLevel()
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
