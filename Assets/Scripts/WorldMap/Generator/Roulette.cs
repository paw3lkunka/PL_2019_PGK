using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roulette<T>
{
    public Roulette(IList<T> objects, IList<int> spawnChances)
    {
        this.objects = objects;
        this.chances = new List<int>();

        for (int i = 0; i < objects.Count; i++)
        {
            int chance = spawnChances[i];
            chances.Add(chance);
        }
    }

    public T GetRandom(int zone)
    {
        int range = 0;

        foreach (var ch in chances)
        {
            range += ch;
        }

        int randomNumber = Random.Range(0, range);
        int index = 0;

        while (index < objects.Count  && chances[index] > 0 && chances[index] < randomNumber)
        {
            randomNumber -= chances[index];
            index++;
        }

        return index < objects.Count ? objects[index] : default;
    }

    private IList<T> objects;
    private List<int> chances;
}
