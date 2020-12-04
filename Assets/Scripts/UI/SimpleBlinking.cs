using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleBlinking : MonoBehaviour
{
    public Image image;
    public float interval;

    private void OnEnable()
    {
        StartCoroutine(Blink());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            image.enabled = !image.enabled;
            yield return new WaitForSeconds(interval);
        }
    }
}
