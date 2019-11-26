using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextsEmitter : MonoBehaviour
{
    public GameObject textPrefab;
    /*
    private void Start()
    {
        IEnumerator Routine()
        {
            while(true)
            {
                Emit("2137", Color.red, 3);
                yield return new WaitForSeconds(.5f);
            }
        }

        StartCoroutine(Routine());
    }
    */

    public void Emit( string text, Color color, float lifeTime)
    {
        GameObject obj = Instantiate(textPrefab, transform.position, Quaternion.identity);
        obj.GetComponent<FloatingText>().Set(text, color, lifeTime);
    }
}
