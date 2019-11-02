using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewMembers : MonoBehaviour
{
    [Range(1, 25)]
    public int crewSize = 1;

    void FixedUpdate(){
        if(crewSize == 1){
            GameObject.Find("GameOverPlane").GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
            GameObject.Find("GameOverText").GetComponent<RectTransform>().sizeDelta = new Vector2(240, 90);
        }
    }
}
