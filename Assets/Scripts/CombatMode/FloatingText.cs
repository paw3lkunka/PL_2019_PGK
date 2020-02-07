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
    [SerializeField] private float speed;
    [SerializeField] private float entropy;
    [SerializeField] private float amplitude;
    [SerializeField] private float speedAcc;
    [SerializeField] private float entropyAcc;
    [SerializeField] private float amplitudeAcc;
#pragma warning restore

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        tmPro = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        transform.Translate(Mathf.Sin(Time.time * entropy) * amplitude, speed, 0);

        lifeTime -= Time.deltaTime;

        speed += Time.deltaTime * speedAcc;
        entropy += Time.deltaTime * entropyAcc;
        amplitude += Time.deltaTime * amplitudeAcc;


        if (lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Component

    public void Set(string text, Color color, float lifeTime)
    {
        tmPro.text = text;
        tmPro.color = color;

        this.lifeTime = lifeTime;
    }

    #endregion
}
