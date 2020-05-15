using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps scene specific configuration for world map
/// </summary>
public class WorldSceneManager : Singleton<WorldSceneManager, ForbidLazyInstancing>
{
#pragma warning disable
    [Tooltip("Cooldown to enter location after exiting one (in seconds)")]
    [SerializeField] private float locationCooldown = 5.0f;
#pragma warning restore
    public bool CanEnterLocations { get; private set; } = true;

    private void Start()
    {
        var leader = GameObject.FindGameObjectWithTag("Leader");
        leader.transform.position = GameplayManager.Instance.lastWorldMapPosition;
        StartCoroutine(LocationCooldown(locationCooldown));
    }

    private IEnumerator LocationCooldown(float delay)
    {
        CanEnterLocations = false;
        yield return new WaitForSeconds(delay);
        CanEnterLocations = true;
    }
}
