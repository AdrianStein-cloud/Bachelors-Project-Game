using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameSettings : Singleton<GameSettings>
{
    private int seed;

    public void SetSeed(int seed)
    {
        if (seed == -1)
        {
            seed = Random.Range(1, 10000000);
        }
        Debug.Log("Seed: " + seed);
    }

    public int GetSeed()
    {
        return seed;
    }
}
