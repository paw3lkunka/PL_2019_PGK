using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObjectOnUpdate : MonoBehaviour
{
    public GameObject objectToFollow;

    void Update()
    {
        transform.position = objectToFollow.transform.position;
    }
}
