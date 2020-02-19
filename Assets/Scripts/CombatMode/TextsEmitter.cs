using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextsEmitter : MonoBehaviour
{
    #region Variables

    public GameObject textPrefab;

    #endregion

    #region MonoBehaviour



    #endregion

    #region Component

    public void Emit(string text, Color color, float lifeTime)
    {
        GameObject obj = Instantiate(textPrefab, transform.position, Quaternion.identity);
        obj.GetComponent<FloatingText>().Set(text, color, lifeTime);
    }

    #endregion
}
