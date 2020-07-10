using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObeliskTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Leader"))
        {
            GameplayManager.Instance.MarkLastLocationAsVisitedObelisk();
            Instantiate(ApplicationManager.Instance.PrefabDatabase.shrineMarker, transform.position, Quaternion.identity);
            Debug.Log("Help me with sound playing please ;-;");
            GetComponent<AudioSource>().Play();
        }
    }
}
