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
    [SerializeField] private bool cameraEffectsEnabled = true;
    [SerializeField] private float cameraEffectMultiplier = 0.05f;
#pragma warning restore

    private Camera mainCamera;
    private float startCameraSize;

    private void Awake()
    {
        mainCamera = Camera.main;
        startCameraSize = mainCamera.orthographicSize;
    }

    private void Update()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, controller.GetBeatAnimationTime());
        mainCamera.orthographicSize = startCameraSize - controller.GetBeatAnimationTime() * cameraEffectMultiplier;
    }
}
