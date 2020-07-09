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

    public GameObject Leader { get; private set; }
    public WorldMapCursor Cursor { get; private set; }
    public ResourceDepleter ResourceDepleter { get; private set; }
    public ResourcesUseIndicator ResUseIndicator { get; private set; }

    private MapGenerator mapGenerator;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        Leader = GameObject.FindGameObjectWithTag("Leader");
        Cursor = FindObjectOfType<WorldMapCursor>();
        mapGenerator = FindObjectOfType<MapGenerator>();
        ResourceDepleter = FindObjectOfType<ResourceDepleter>();
        ResUseIndicator = FindObjectOfType<ResourcesUseIndicator>();

        Vector3 exitOffset = LocationCentre.exitDirection * GameplayManager.Instance.lastLocationRadius;
        Leader.GetComponent<NavMeshAgent>().Warp(GameplayManager.Instance.lastLocationPosition + exitOffset);

        StartCoroutine(LocationCooldown(locationCooldown));

        mapGenerator.seed = GameplayManager.Instance.mapGenerationSeed;
        mapGenerator.useCustomSeed = true;
        mapGenerator.Generate();

        UIOverlayManager.Instance?.ControlsSheet.Clear();
        UIOverlayManager.Instance?.ControlsSheet.AddSheetElement(ButtonActionType.Walk, "Choose destination");
        UIOverlayManager.Instance?.ControlsSheet.AddSheetElement(ButtonActionType.Pause, "Pause menu");
    }

    private void OnEnable()
    {
        ApplicationManager.Instance.Input.CombatMode.Disable();
    }

    private void OnDisable()
    {
        ApplicationManager.Instance.Input.CombatMode.Enable();
    }

    private IEnumerator LocationCooldown(float delay)
    {
        CanEnterLocations = false;
        yield return new WaitForSeconds(delay);
        CanEnterLocations = true;
    }
}
