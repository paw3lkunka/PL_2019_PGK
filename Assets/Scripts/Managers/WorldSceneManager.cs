using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    [HideInInspector]
    public GameObject leader;

    /// <summary>
    /// Should be near to locations size;
    /// </summary>
    public float locationBorderRadius;

    private MapGenerator mapGenerator;

    protected override void Awake()
    {
        base.Awake();
        mapGenerator = FindObjectOfType<MapGenerator>();
    }

    private void Start()
    {
        leader = GameObject.FindGameObjectWithTag("Leader");

        Vector3 exitOffset = Vector3.forward * locationBorderRadius;
        exitOffset = Quaternion.AngleAxis(ExitZone.angle, Vector3.up) * exitOffset;
        Debug.Break();
        leader.GetComponent<NavMeshAgent>().Warp(GameplayManager.Instance.lastLocationPosition + exitOffset);

        StartCoroutine(LocationCooldown(locationCooldown));

        mapGenerator.seed = GameplayManager.Instance.mapGenerationSeed;
        mapGenerator.useCustomSeed = true;
        mapGenerator.Generate();

        UIOverlayManager.Instance?.ControlsSheet.Clear();
        UIOverlayManager.Instance?.ControlsSheet.AddSheetElement(ButtonActionType.Walk, "Choose destination");
        UIOverlayManager.Instance?.ControlsSheet.AddSheetElement(ButtonActionType.Pause, "Pause menu");
    }

    private IEnumerator LocationCooldown(float delay)
    {
        CanEnterLocations = false;
        yield return new WaitForSeconds(delay);
        CanEnterLocations = true;
    }
}
