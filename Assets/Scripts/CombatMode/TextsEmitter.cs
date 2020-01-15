using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextsEmitter : MonoBehaviour
{
    public GameObject textPrefab;

    public void Emit( string text, Color color, float lifeTime)
    {
        GameObject obj = Instantiate(textPrefab, transform.position, Quaternion.identity);
        obj.GetComponent<FloatingText>().Set(text, color, lifeTime);
    }
}
