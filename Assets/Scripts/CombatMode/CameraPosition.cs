using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    #region Variables



    #endregion

    #region MonoBehaviour

    private void LateUpdate()
    {

        if (GameManager.Instance.cultistNumber > 0)
        {
            var crew = GameManager.Instance.ourCrew;
            if (GameManager.Instance.cultistNumber == 0)
            {
                return;
            }
            else if (GameManager.Instance.cultistNumber == 1)
            {
                transform.position = (Vector2)crew[0].transform.position;
                transform.position += Vector3.back * 10;
            }
            else
            {
                Vector2 min = Vector2.positiveInfinity;
                Vector2 max = Vector2.negativeInfinity;

                foreach (GameObject obj in crew)
                {
                    Vector2 pos = obj.transform.position;

                    max.x = Mathf.Max(max.x, pos.x);
                    max.y = Mathf.Max(max.y, pos.y);
                    min.x = Mathf.Min(min.x, pos.x);
                    min.y = Mathf.Min(min.y, pos.y);

                    transform.position = min + (max - min) / 2;
                    transform.position += Vector3.back * 10;
                }
            }
        }
    }

    #endregion

    #region Component



    #endregion
}
