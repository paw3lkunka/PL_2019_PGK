using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecruitUIController : MonoBehaviour
{
    public static bool isRecruitUIOn = false;
    public Recruitable recruit;
    public TextMeshProUGUI costText;

    private void Start()
    {
        costText.text = "Faith cost: " + recruit.FaithCost;
        isRecruitUIOn = true;
    }

    public void Recruit()
    {
        if(recruit.Recruit())
        { 
            BehaviourUserInput.controlEnabled = true;
            isRecruitUIOn = false;
            UIOverlayManager.Instance.PopFromCanvas();
        }
        else
        {
            UIOverlayManager.Instance.PushToCanvas(ApplicationManager.Instance.PrefabDatabase.insufficientFundsDialog, PushBehaviour.Lock);
        }
    }

    public void Cancel()
    {
        BehaviourUserInput.controlEnabled = true;
        isRecruitUIOn = false;
        UIOverlayManager.Instance.PopFromCanvas();
    }
}
