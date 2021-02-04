using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roulette<T>
{
    public Roulette(IList<T> objects, IList<SpawnChance> spawnChances, SpawnChance emptyChance = null)
    {
        this.objects = objects;
        this.chances = new List<SpawnChance>();

        for (int i = 0; i < objects.Count; i++)
        {
            SpawnChance chance = spawnChances[i];
            chances.Add(chance);
        }

        chances.Add(emptyChance ?? new SpawnChance());
    }

    public T GetRandom(int zone)
    {
        int range = 1;

        foreach (var ch in chances)
        {
            range += ch.forZone[zone];
        }

        int randomNumber = Random.Range(0, range);
        int index = 0;

        while (index < objects.Count && chances[index].forZone[zone] < randomNumber)
        {
            randomNumber -= chances[index].forZone[zone];
            index++;
        }

        return index < objects.Count ? objects[index] : default;
    }

    private IList<T> objects;
    private List<SpawnChance> chances;
}
