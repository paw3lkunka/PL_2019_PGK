using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    public bool fadeOnStart = true;
    public float time = 2.0f;
    private Image fadeImage;

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float timeElapsed = 0.0f;
        Color fadeColor = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1.0f);

        while (timeElapsed < time)
        {
            fadeColor.a = Mathf.Lerp(startAlpha, endAlpha, timeElapsed / time);
            fadeImage.color = fadeColor;
            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private void Start()
    {
        fadeImage = GetComponent<Image>();

        if (fadeOnStart)
        {
            StartCoroutine(Fade(1.0f, 0.0f));
        }
    }
}
