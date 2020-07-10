using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObeliskTrigger : MonoBehaviour
{
    private bool IsAlreadyActivated { get => GameplayManager.Instance.visitedObelisksIds.Contains(GameplayManager.Instance.lastLocationId); }

    private GameObject SpawnLigth() => Instantiate(ApplicationManager.Instance.PrefabDatabase.shrineMarker, transform.position, Quaternion.identity);

    private void Start()
    {
        if (IsAlreadyActivated)
        {
            SpawnLigth();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Leader") && !IsAlreadyActivated)
        {
            GameplayManager.Instance.MarkLastLocationAsVisitedObelisk();
            SpawnLigth();
            Debug.Log("Help me with sound playing please ;-;");
            GetComponent<AudioSource>().Play();
        }
    }
}
