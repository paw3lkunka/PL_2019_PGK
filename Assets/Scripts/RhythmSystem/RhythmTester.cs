using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RhythmTester : MonoBehaviour
{
    #region Variables

#pragma warning disable
    [SerializeField] private RhythmController controller;
    [SerializeField] private Image rhythmIndicator;
    [SerializeField] private Color badColor = Color.red;
    [SerializeField] private Color goodColor = Color.yellow;
    [SerializeField] private Color greatColor = Color.green;
#pragma warning restore

    private Beat beatStatus;
    private Color transparent = new Color(0, 0, 0, 0);

    private NewInput input;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        input = GameManager.Instance.input;
    }

    private void OnEnable()
    {
        input.Gameplay.SetWalkTarget.performed += HitBeatInputTest;
        input.CombatMode.SetShootTarget.performed += HitBeatInputTest;
    }

    private void OnDisable()
    {
        input.Gameplay.SetWalkTarget.performed -= HitBeatInputTest;
        input.CombatMode.SetShootTarget.performed -= HitBeatInputTest;
    }


    #endregion

    #region Component



    #endregion

    #region Input

    private void HitBeatInputTest(InputAction.CallbackContext ctx)
    {
        beatStatus = controller.HitBeat();
    }

    #endregion
}
