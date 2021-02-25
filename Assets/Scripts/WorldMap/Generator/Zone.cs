using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Zone
{
    public int minObsacles;
    public int maxObstacles;
    public List<PrefabWrapper> locations;
    public List<PrefabWrapper> obstacles;
}

[Serializable]
public class PrefabWrapper
{
    public GameObject prefab;
    public float spawnChance = 1;
}