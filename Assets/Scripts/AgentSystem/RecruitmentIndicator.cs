using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Recruitable))]
public class RecruitmentIndicator : MonoBehaviour
{
    public Image recruitmentBar;
    public Image cantRecruitImage;

    private Recruitable recruitable;

    private void Awake()
    {
        recruitable = GetComponent<Recruitable>();
    }

    private void Update()
    {
        if (recruitable.WasAffectedOnce)
        {
            if (recruitable.CanBeRecruited)
            {
                cantRecruitImage.enabled = false;
                recruitmentBar.enabled = true;
                recruitmentBar.fillAmount = recruitable.RecruitmentProgress;
            }
            else
            {
                cantRecruitImage.enabled = true;
                recruitmentBar.enabled = false;
            }
        }
        else
        {
            cantRecruitImage.enabled = false;
            recruitmentBar.enabled = false;
        }
    }
}
