using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMouseClick : MonoBehaviour
{
    public float speed = 1.0f;
    Vector2 targetPos;
    
    private void Start()
    {
        targetPos = transform.position;
    }
    
    void Update ()
    {
        if(Input.GetMouseButtonDown(0))
            targetPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if((Vector2)transform.position != targetPos)
        {
            DottedLine.Instance.DrawDottedLine(targetPos, transform.position);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
    }
}
