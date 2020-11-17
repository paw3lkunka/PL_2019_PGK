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

        if (!GameplayManager.Instance.firstTimeOnMap)
        {
            Vector3 exitOffset = LocationCentre.exitDirection * GameplayManager.Instance.lastLocationRadius;
            Leader.GetComponent<NavMeshAgent>().Warp(GameplayManager.Instance.lastLocationPosition + exitOffset);
        }

        GameplayManager.Instance.firstTimeOnMap = false;
        StartCoroutine(LocationCooldown(locationCooldown));

        mapGenerator.seed = GameplayManager.Instance.mapGenerationSeed;
        mapGenerator.useCustomSeed = true;
        mapGenerator.Generate();

        MarkShrine();

        UIOverlayManager.Instance?.ControlsSheet.Clear();
        UIOverlayManager.Instance?.ControlsSheet.AddSheetElement(ButtonActionType.Walk, "Choose destination");
        UIOverlayManager.Instance?.ControlsSheet.AddSheetElement(ButtonActionType.Pause, "Pause menu");
        UIOverlayManager.Instance?.ControlsSheet.AddSheetElement(ButtonActionType.CameraMove, "Move camera");
        UIOverlayManager.Instance?.ControlsSheet.AddSheetElement(ButtonActionType.CameraZoom, "Zoom camera");
    }

    private void OnEnable()
    {
        ApplicationManager.Instance.Input.Gameplay.Pause.performed += WorldMapPause;
        ApplicationManager.Instance.Input.Gameplay.Pause.Enable();
        ApplicationManager.Instance.Input.CombatMode.Disable();
    }

    private void OnDisable()
    {
        ApplicationManager.Instance.Input.Gameplay.Pause.performed -= WorldMapPause;
        ApplicationManager.Instance.Input.Gameplay.Pause.Disable();
        ApplicationManager.Instance.Input.CombatMode.Enable();
    }

    private void WorldMapPause(InputAction.CallbackContext ctx)
    {
        GameplayManager.Instance.TogglePause();
    }

    private IEnumerator LocationCooldown(float delay)
    {
        CanEnterLocations = false;
        yield return new WaitForSeconds(delay);
        CanEnterLocations = true;
    }

    private void MarkShrine()
    {
        if(GameplayManager.Instance.obeliskActivated && GameplayManager.Instance.markedShrineId == default)
        {
            var shrines = GameObject.FindGameObjectsWithTag("Shrine").Where((obj) => !GameplayManager.Instance.visitedShrinesIds.Contains(obj.GetComponent<Location>().id));

            float nearestDistance = float.PositiveInfinity;
            GameObject nearestLocation = null;

            foreach(var shrine in shrines)
            {
                var dist = Vector3.Distance(shrine.transform.position, GameplayManager.Instance.lastLocationPosition);
                if( dist < nearestDistance)
                {
                    nearestDistance = dist;
                    nearestLocation = shrine;
                }
            }
            GameplayManager.Instance.markedShrineId = nearestLocation.GetComponent<Location>().id;
        }

        if(GameplayManager.Instance.markedShrineId != default)
        {
            var shrine = GameObject.FindGameObjectsWithTag("Shrine").Where((obj) => obj.GetComponent<Location>().id == GameplayManager.Instance.markedShrineId).First();
            Instantiate(ApplicationManager.Instance.PrefabDatabase.shrineMarker, shrine.transform.position, Quaternion.identity);
        }
    }
}
