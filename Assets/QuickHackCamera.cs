using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickHackCamera : MonoBehaviour
{
    Transform leader;
    void Start()
    {
        // Nie jestem z tego dumny
        leader = GameObject.Find("Cultist Leader(Clone)").transform;
    }
    private void Update()
    {
        try
        {
            transform.position = new Vector3(leader.position.x, leader.position.y, transform.position.z);
        }
        catch
        {
            // ani z tego
        }
    }   
}
