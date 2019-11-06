using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEncounter : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float probabilityFactor = 0.1f;
    
    private Vector2 playerLastPosition;
    private float timeSinceLastEncounter;
    private float probability;
    private bool encounter;

    //WORK IN PROGRESS... 
    void Start(){
        playerLastPosition = transform.position;
        timeSinceLastEncounter = Time.time;
        probability = 0.0f;
        encounter = false;
    }
    
    void Update ()
    {
        if((Time.time - timeSinceLastEncounter) > 20.0f &&
            (playerLastPosition.x != transform.position.x || playerLastPosition.y != transform.position.y))
        {
            probability += Random.Range(0, probabilityFactor);

            if(probability >= 1.0f)
                encounter = true;
            
            if(encounter)
            {
                Debug.Log("Tu bedzie Encounter jak go ktos zrobi xDDDD");
                timeSinceLastEncounter = Time.time;
                probability = 0.0f;
                encounter = false;
            }
        }
        playerLastPosition = transform.position;
    }
}
