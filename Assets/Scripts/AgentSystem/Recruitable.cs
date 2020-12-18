using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Recruitable : MonoBehaviour
{
    private readonly float recruitmentChance = 0.5f;
    private bool canBeRecruited;
    private float recruitmentProgress;


    private void Awake()
    {
        // Randomize if the recruit can be recruited at all
        canBeRecruited = recruitmentChance < Random.Range(0.0f, 1.0f);
    }


}
