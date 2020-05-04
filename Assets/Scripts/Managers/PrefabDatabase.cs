using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabDatabase", menuName = "Database/PrefabDatabase", order = 0)]
public class PrefabDatabase : ScriptableObject 
{
    [Header("Cursor prefabs")]
    public GameObject cursorPrefab;
    
    [Header("Entities")]
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

    [Header("World space UI")]
    public GameObject healthBar;
    public GameObject floatingTextEmmiter;
    public GameObject floatingTextLife;
    public GameObject floatingTextFaith;
}
