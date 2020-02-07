using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMouseClick : MonoBehaviour
{
    #region Variables

    public float speed = 1.0f;
    private Vector2 targetPos;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        targetPos = GameManager.Instance.savedPosition;
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.Gui.isMouseOver)
            targetPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if ((Vector2)transform.position != targetPos && GameManager.Instance.cultistNumber > 0)
        {
            DottedLine.Instance.DrawDottedLine(targetPos, transform.position);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
    }

    #endregion

    #region Component



    #endregion
}
