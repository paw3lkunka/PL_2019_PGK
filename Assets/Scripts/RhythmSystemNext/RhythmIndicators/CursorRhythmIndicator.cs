using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorRhythmIndicator : MonoBehaviour
{
    #region Variables

#pragma warning disable
    [Header("Rhythm pulse indicator")]
    [SerializeField] private SpriteRenderer rhythmIndicator;
    [SerializeField] private AnimationCurve rhythmCurve;
    [SerializeField] private float rhytmIndicatorMaxSize = 5.0f;

    [Space]
    
    [Header("Hit indicator")]
    [SerializeField] private SpriteRenderer hitIndicator;
    [SerializeField] private float animationTime;
    [SerializeField] private Color badHitColor = Color.red;
    [SerializeField] private Color goodHitColor = Color.cyan;
    [SerializeField] private Color greatHitColor = Color.green;
    [SerializeField] private Color perfectHitColor = Color.magenta;
    [SerializeField] private AnimationCurve hitCurve;
    [SerializeField] private float hitIndicatorMaxSize = 3.0f;
    
    [Space]
    
    [Header("Camera effects")]
    [SerializeField] private bool cameraEffectsEnabled = true;
    [SerializeField] private AnimationCurve cameraCurve;
    [SerializeField] private float cameraEffectMultiplier = 0.05f;
    [SerializeField] private float rageMultiplier = 2.0f;
#pragma warning restore

    private Camera mainCamera;
    private float startCameraSize;
    private float coroutineTime;

    private bool cameraEffectsEnabledInternal = false;
    private float cameraEffectMultiplierInternal;

    #endregion

    #region MonoBehaviour

    private void Update()
    {
        if (AudioTimeline.Instance)
        {
            var normalizedTimeUntilNextBeat = AudioTimeline.Instance.NormalizedTimeUntilNextBeat;

            var rhythmSize = rhytmIndicatorMaxSize - ((rhytmIndicatorMaxSize - 1) * normalizedTimeUntilNextBeat);
            rhythmIndicator.transform.localScale = new Vector3(rhythmSize, rhythmSize, 0);

            var hitSize = hitIndicatorMaxSize - ((hitIndicatorMaxSize - 1) * normalizedTimeUntilNextBeat);
            hitIndicator.transform.localScale = new Vector3(hitSize, hitSize, 0);

            rhythmIndicator.color = new Color(1.0f, 1.0f, 1.0f, rhythmCurve.Evaluate(normalizedTimeUntilNextBeat));

            if (cameraEffectsEnabled && cameraEffectsEnabledInternal)
            {
                mainCamera.orthographicSize = startCameraSize - cameraCurve.Evaluate(normalizedTimeUntilNextBeat) * cameraEffectMultiplierInternal;
            }
        }
    }

    #endregion

    #region Component



    #endregion
}
