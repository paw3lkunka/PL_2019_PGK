using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class RhythmAnimator : MonoBehaviour
{
#pragma warning disable
    [SerializeField] private Image image;
    [SerializeField] private RhythmController controller;
#pragma warning restore

    private void Update()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, controller.GetBeatAnimationTime());
    }
}
