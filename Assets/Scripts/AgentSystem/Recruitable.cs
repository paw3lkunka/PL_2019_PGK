using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Recruitable : MonoBehaviour
{
    // Const fields because this values are for now assumed global
    private const float RecruitmentChance = 0.5f;
    private const float RecruitmentSpeed = 0.2f;
    private bool isAffectedByCult;
    public string recruitmentTriggerTag = "RecruitTrigger";
    public bool CanBeRecruited { get; private set; } = false;
    public bool WasAffectedOnce { get; private set; } = false;

    public float RecruitmentProgress { get; private set; } = 0.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(recruitmentTriggerTag))
        {
            if (!WasAffectedOnce)
            {
                // Randomize if the recruit can be recruited at all
                CanBeRecruited = RecruitmentChance < Random.Range(0.0f, 1.0f);
                WasAffectedOnce = true;
            }
            isAffectedByCult = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(recruitmentTriggerTag))
        {
            isAffectedByCult = false;
        }
    }

    private bool Recruit()
    {
        var newCultist = new CultistEntityInfo(ApplicationManager.Instance.PrefabDatabase.cultists[0]);
        newCultist.Instantiate(transform.position, transform.rotation);
        GameplayManager.Instance.cultistInfos.Add(newCultist);

        Destroy(gameObject);
        return true;
    }

    private void Update()
    {
        if (CanBeRecruited)
        {
            if (isAffectedByCult && RecruitmentProgress <= 1.0f)
            {
                RecruitmentProgress += RecruitmentSpeed * Time.deltaTime;
            }
            else if (!isAffectedByCult && RecruitmentProgress > 0.0f)
            {
                RecruitmentProgress -= RecruitmentSpeed * Time.deltaTime; ;
            }

            if (RecruitmentProgress >= 1.0f)
            {
                Recruit();
            }

            Mathf.Clamp01(RecruitmentProgress);
        }
    }
}
