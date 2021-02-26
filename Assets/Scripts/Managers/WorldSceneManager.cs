using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System;
using UnityEngine.InputSystem;

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
        QualitySettings.shadowDistance = 300.0f;
    }

    private void Start()
    {
        Leader = GameObject.FindGameObjectWithTag("Leader");
        Cursor = FindObjectOfType<WorldMapCursor>();
        mapGenerator = FindObjectOfType<MapGenerator>();
        ResourceDepleter = FindObjectOfType<ResourceDepleter>();
        ResUseIndicator = FindObjectOfType<ResourcesUseIndicator>();

        GameplayManager.Instance.Faith.Overflowable = false;

        if (!GameplayManager.Instance.firstTimeOnMap)
        {
            Vector3 exitOffset = LocationManager.exitDirection * GameplayManager.Instance.lastLocationRadius;
            Leader.GetComponent<NavMeshAgent>().Warp(GameplayManager.Instance.lastLocationPosition + exitOffset);
        }

        GameplayManager.Instance.firstTimeOnMap = false;
        StartCoroutine(LocationCooldown(locationCooldown));

        mapGenerator.seed = GameplayManager.Instance.mapGenerationSeed;
        mapGenerator.Generate();

        UIOverlayManager.Instance?.ControlsSheet.Clear();
        UIOverlayManager.Instance?.ControlsSheet.AddSheetElement(ButtonActionType.Walk, "Choose destination");
        UIOverlayManager.Instance?.ControlsSheet.AddSheetElement(ButtonActionType.Shoot, "Replenish health");
        UIOverlayManager.Instance?.ControlsSheet.AddSheetElement(ButtonActionType.Pause, "Pause menu");
        UIOverlayManager.Instance?.ControlsSheet.AddSheetElement(ButtonActionType.CameraMove, "Move camera");
        UIOverlayManager.Instance?.ControlsSheet.AddSheetElement(ButtonActionType.CameraZoom, "Zoom camera");
    }

    private void Update()
    {
        if (Leader.transform.position.magnitude >= GameplayManager.Instance.escapeDistance)
        {
            ApplicationManager.Instance.GameOver(true);
        }
    }

    private IEnumerator LocationCooldown(float delay)
    {
        CanEnterLocations = false;
        yield return new WaitForSeconds(delay);
        CanEnterLocations = true;
    }
}
