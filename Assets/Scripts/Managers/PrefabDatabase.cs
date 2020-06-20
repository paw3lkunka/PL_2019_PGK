﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PrefabDatabase", menuName = "Database/PrefabDatabase", order = 0)]
public class PrefabDatabase : ScriptableObject
{
    [Header("Managers")]
    public GameObject applicationManager;
    public GameObject gameplayManager;
    public GameObject worldSceneManager;

    [Header("Cursor prefabs")]
    public GameObject cursorPrefab;

    [Header("Entities")]
    public GameObject projectile;
    public GameObject cultLeader;
    public List<GameObject> cultists;
    public List<GameObject> enemies;

    [Header("Main menu")]
    public GameObject mainMenuGUI;
    public GameObject optionsMenuGUI;
    public GameObject difficultyMenuGUI;

    [Header("Gui prefabs")]
    public GameObject lockGUI;
    public GameObject combatSceneGUI;
    public GameObject worldSceneGUI;
    public GameObject rhythmGUI;
    public GameObject pauseGUI;
    public GameObject controlsSheet;
    public GameObject controlsSheetElement;
    public GameObject recruitGUI;
    public GameObject insufficientFundsDialog;

    [Header("World space UI")]
    public GameObject healthBar;
    public GameObject floatingTextEmitter;
    public GameObject floatingTextResourceLost;
    public GameObject floatingTextResourceGained;

    [Header("Locations and enviro")]
    public List<GameObject> stdLocations;
    public List<GameObject> shrines;
    public List<GameObject> enviro;

    public static PrefabDatabase Load {get => Resources.Load("PrefabDatabase") as PrefabDatabase;}
}
