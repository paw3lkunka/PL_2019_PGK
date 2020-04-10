using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnd : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CultLeader>())
        {
            StartCoroutine(Routine());
            IEnumerator Routine()
            {
                yield return null;
                GameManager.Instance.won = true;
                GameManager.Instance.GameOver();
            }
        }
    }
}
