using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameSettings : Singleton<GameSettings>
{
    private int seed;

    public void SetSeed(int seed)
    {
        this.seed = seed;
    }

    public int GetSeed()
    {
        return seed;
    }
}
