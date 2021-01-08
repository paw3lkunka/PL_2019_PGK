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
    public new ParticleSystem particleSystem;

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

                if (!particleSystem.isPlaying && recruitable.IsBeingRecruited)
                    particleSystem.Play();
                else if (particleSystem.isPlaying && !recruitable.IsBeingRecruited)
                    particleSystem.Stop();
                    

                recruitmentBar.fillAmount = recruitable.RecruitmentProgress;
            }
            else
            {
                cantRecruitImage.enabled = true;
                recruitmentBar.enabled = false;

                if (particleSystem.isPlaying && !recruitable.IsBeingRecruited)
                    particleSystem.Stop();
            }
        }
        else
        {
            cantRecruitImage.enabled = false;
            recruitmentBar.enabled = false;

            if (particleSystem.isPlaying)
                particleSystem.Stop();
        }
    }
}
