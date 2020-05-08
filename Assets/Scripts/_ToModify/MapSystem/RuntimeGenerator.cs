using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeGenerator : MonoBehaviour
{
    #region Variables

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

    public int mainSeed;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        GameplayManager.Instance.OnLocationEnter += OnLocationEnter;
        GameplayManager.Instance.OnLocationExit += OnLocationExit;
        ApplicationManager.Instance.GameOverEvent += OnGameOver;
    }
    
    private void OnDestroy()
    {
        GameplayManager.Instance.mapGenerated = false;
        GameplayManager.Instance.OnLocationEnter -= OnLocationEnter;
        GameplayManager.Instance.OnLocationExit -= OnLocationExit;
        ApplicationManager.Instance.GameOverEvent -= OnGameOver;
    }

    private void Start()
    {
        if (!GameplayManager.Instance.mapGenerated)
        {
            GameplayManager.Instance.mapGenerated = true;

            if (!useCustomMainSeed)
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
                for (int i = 1; i < backgroundIterations; i++)
                {
                    backgroundSeeds[i] = Random.Range(int.MinValue, int.MaxValue);
                }
            }

            if (clearPreSelected)
            {
                try
                {
                    foreground.Clear();
                }
                catch (System.NullReferenceException)
                {
                    //do nothing, ofcourse xD
                }
                foreground.useCustomSeed = true;
                try
                {
                    background.Clear();
                }
                catch (System.NullReferenceException)
                {
                    //do nothiung
                }
                background.useCustomSeed = true;
            }

            foreground.seed = (int)forgroundSeed;
            foreground.Generate();

            for (int i = 0; i < backgroundIterations; i++)
            {
                background.seed = backgroundSeeds[i];
                background.Generate();
            }

            if (reshuffleEachTime)
            {
                forgroundSeed = null;
                backgroundSeeds = null;
            }

        }        
    }

    #endregion

    #region Component

    private void OnLocationEnter()
    {
        gameObject.SetActive(false);
    }

    private void OnLocationExit()
    {
        gameObject.SetActive(true);
    }

    private void OnGameOver()
    {
        Destroy(gameObject);
    }

    #endregion
}
