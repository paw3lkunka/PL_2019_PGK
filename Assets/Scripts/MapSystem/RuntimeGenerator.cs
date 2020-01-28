using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeGenerator : MonoBehaviour
{
    public int mainSeed;
    
    [Space]
    public MapGenerator foreground;
    public MapGenerator background;

    [Space]
    public bool useCustomMainSeed = false;
    public bool overrideInitialSeed = true;
    public bool clearPreSelected = true;
    public bool reshuffleEachTime = false;

    [Space]
    public int backgroundIterations = 1;

    private static int? forgroundSeed = null;
    private static int[] backgroundSeeds = null;
    
    // Start is called before the first frame update
    void Start()
    {
        if(!useCustomMainSeed)
        {
            mainSeed = Random.Range(int.MinValue, int.MaxValue);
        }

        Random.InitState(mainSeed);

        if (forgroundSeed == null)
        {
            forgroundSeed = overrideInitialSeed ? Random.Range(int.MinValue, int.MaxValue) : foreground.seed;
        }

        if (backgroundSeeds == null)
        {
            backgroundSeeds = new int[backgroundIterations];
            backgroundSeeds[0] = overrideInitialSeed ? Random.Range(int.MinValue, int.MaxValue) : background.seed;
            for(int i=1; i<backgroundIterations; i++)
            {
                backgroundSeeds[i] = Random.Range(int.MinValue, int.MaxValue);
            }
        }

        if (clearPreSelected)
        {
            foreground.Clear();
            foreground.useCustomSeed = true;
            background.Clear();
            background.useCustomSeed = true;
        }

        foreground.seed = (int)forgroundSeed;
        foreground.Generate();

        for (int i = 0; i < backgroundIterations; i++)
        {
            background.seed = backgroundSeeds[i];
            background.Generate();
        }
               
        if(reshuffleEachTime)
        {
            forgroundSeed = null;
            backgroundSeeds = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
