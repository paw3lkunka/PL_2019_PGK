using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorUsage : MonoBehaviour
{
    private const int maxHeight = 128;
    public float usageFactor = 0.0002f;
    public GameObject indicator;
    public bool isFaith = false;
    
    private RawImage indicatorImage;
    private float indicatorAmount = 1.0f;
    private Vector2 playerLastPosition;
    private float timeLastMemberDied = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        indicatorImage = indicator.GetComponent<RawImage>();
        if(isFaith)
        {
            indicatorAmount = 0.5f;
            indicatorImage.GetComponent<RectTransform>().sizeDelta = new Vector2(32, maxHeight * indicatorAmount);
        }
        playerLastPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            indicatorAmount = 1.0f;
        }
        if(playerLastPosition.x != transform.position.x || playerLastPosition.y != transform.position.y)
        {
            indicatorAmount -= usageFactor * (GetComponent<CrewMembers>().crewSize / 5);
            if(isFaith)
            {
                //"Faith strenghtening"
                indicatorAmount += usageFactor * (GetComponent<CrewMembers>().crewSize / 7);
            }
            indicatorImage.GetComponent<RectTransform>().sizeDelta = new Vector2(32, maxHeight * indicatorAmount);
            

            if( indicatorAmount < 0.3f && 
                (Time.time - timeLastMemberDied) > (25.0f * (indicatorAmount / 0.3f)) )
            {
                GetComponent<CrewMembers>().crewSize -= 1;
                timeLastMemberDied = Time.time;
            }

            if( isFaith && indicatorAmount > 0.9f 
            && (Time.time - timeLastMemberDied) > ( 15.0f * (1.0 - (indicatorAmount / 0.9f - 1.0f)) ) )
            {
                Debug.Log("New time: " + 15.0f * (1.0 - (indicatorAmount / 0.9f - 1.0f)));
                GetComponent<CrewMembers>().crewSize -= 1;
                timeLastMemberDied = Time.time;
            }
            
            playerLastPosition = transform.position;
        }
    }
}
