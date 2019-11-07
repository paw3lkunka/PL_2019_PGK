using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptStartHack : MonoBehaviour
{
    public GameObject rhythmController;

    void Start()
    {
        StartCoroutine("StartRhythm");
    }

    private IEnumerator StartRhythm()
    {
        yield return new WaitForSeconds(0.0001f);
        rhythmController.SetActive(true);
    }
}
