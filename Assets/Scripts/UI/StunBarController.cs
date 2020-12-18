using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StunBarController : MonoBehaviour
{
    public Image barImage;
    Graphic[] graphics;

    private bool Show
    {
        set
        {
            foreach (var graphic in graphics)
            {
                graphic.enabled = value;
            }
        }
    }

    private void OnEnable()
    {
        graphics = GetComponentsInChildren<Graphic>();
        Show = false;
        AudioTimeline.Instance.OnBeatFail += OnBeatFail;
    }

    private void OnDisable()
    {
        if(AudioTimeline.Instance != null)
        {
            AudioTimeline.Instance.OnBeatFail -= OnBeatFail;
        }
    }

    private void OnBeatFail()
    {
        if (LocationManager.Instance.CanStun)
        {
            Show = true;

            IEnumerator Routine()
            {
                yield return new WaitForSeconds(LocationManager.Instance.stunCooldown);
                Show = false;
            }
            StartCoroutine(Routine());
        }
    }

    private void LateUpdate()
    {
        barImage.fillAmount = LocationManager.Instance.stunCounter / LocationManager.Instance.stunCooldown;
    }
}
