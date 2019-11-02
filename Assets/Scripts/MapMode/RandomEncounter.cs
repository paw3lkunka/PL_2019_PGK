using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEncounter : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float probabilityFactor = 0.1f;
    
    private Vector2 playerLastPosition;
    private float timeSinceLastEncounter;

    //WORK IN PROGRESS... 
    // void Start(){
    //     playerLastPosition = transform.position;
    //     timeSinceLastEncounter = 0.0f;
    // }
    
    // void Update ()
    // {
    //     if(playerLastPosition.x != transform.position.x || playerLastPosition.y != transform.position.y)
    //     {
            
    //         timeSinceLastEncounter = Time.time;
    //     }
    // }
}
