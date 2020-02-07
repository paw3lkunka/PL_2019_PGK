using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Location : MonoBehaviour
{
    #region Variables

    public int generationID;

    #endregion

    #region MonoBehaviour

    private void OnValidate()
    {
        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
    }

    #endregion

    #region Component



    #endregion
}
