using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFaithHack : MonoBehaviour
{

    private void Update()
    {
        GameManager.Instance.Faith = value;
    }

    public float value;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
