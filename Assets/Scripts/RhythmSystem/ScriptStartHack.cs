using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptStartHack : MonoBehaviour
{
    #region Variables

    public GameObject rhythmController;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        StartCoroutine("StartRhythm");
    }

    #endregion

    #region Component

    private IEnumerator StartRhythm()
    {
        yield return new WaitForSeconds(0.0001f);
        rhythmController.SetActive(true);
    }

    #endregion
}
