using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    #region Variables

    private TextMeshProUGUI tmPro;

#pragma warning disable
    [SerializeField] private float lifeTime;
    [SerializeField] private float fadeOutTime;
    [SerializeField] private float speed;
    [SerializeField] private float entropy;
    [SerializeField] private float amplitude;
    [SerializeField] private float speedAcc;
    [SerializeField] private float entropyAcc;
    [SerializeField] private float amplitudeAcc;
#pragma warning restore

    private float fadeOutTimer;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        fadeOutTimer = fadeOutTime;
        tmPro = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        transform.Translate(0, speed, Mathf.Sin(Time.time * entropy) * amplitude);

        lifeTime -= Time.deltaTime;

        speed += Time.deltaTime * speedAcc;
        entropy += Time.deltaTime * entropyAcc;
        amplitude += Time.deltaTime * amplitudeAcc;


        if (lifeTime < 0)
        {
            fadeOutTimer -= Time.deltaTime;

            Color nColor = tmPro.color;
            nColor.a = fadeOutTimer / fadeOutTime;
            tmPro.color = nColor;

            tmPro.color /= fadeOutTime;

            if (fadeOutTimer < 0)
            {
                Destroy(gameObject);
            }

        }
    }

    #endregion

    public void Set(string text)
    {
        tmPro.text = text;
    }

}
