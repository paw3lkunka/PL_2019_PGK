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
    private bool wasAffectedThisFrame = false;
    public string recruitmentTriggerTag = "RecruitTrigger";
    public bool CanBeRecruited { get; private set; } = false;
    public bool WasAffectedOnce { get; private set; } = false;
    public bool IsBeingRecruited => wasAffectedThisFrame && CanBeRecruited;
    public float RecruitmentProgress { get; private set; } = 0.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(recruitmentTriggerTag))
        {
            if (!WasAffectedOnce)
            {
                // Randomize if the recruit can be recruited at all
                CanBeRecruited = GameplayManager.Instance.Faith.Normalized > Random.Range(0.0f, 1.0f);
                WasAffectedOnce = true;
            }
            wasAffectedThisFrame = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(recruitmentTriggerTag))
        {
            wasAffectedThisFrame = true;
        }
    }

    private void Recruit()
    {
        var newCultist = new CultistEntityInfo(ApplicationManager.Instance.PrefabDatabase.cultists[0]);
        newCultist.Instantiate(transform.position, transform.rotation);
        GameplayManager.Instance.cultistInfos.Add(newCultist);

        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (CanBeRecruited)
        {
            if (wasAffectedThisFrame && RecruitmentProgress <= 1.0f)
            {
                RecruitmentProgress += RecruitmentSpeed * Time.fixedDeltaTime;
            }
            else if (!wasAffectedThisFrame && RecruitmentProgress > 0.0f)
            {
                RecruitmentProgress -= RecruitmentSpeed * Time.fixedDeltaTime;
            }

            if (RecruitmentProgress >= 1.0f)
            {
                Recruit();
            }

            wasAffectedThisFrame = false;
            Mathf.Clamp01(RecruitmentProgress);
        }
    }
}
