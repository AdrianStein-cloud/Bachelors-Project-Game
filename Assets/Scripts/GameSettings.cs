using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class GameSettings : Singleton<GameSettings>
{
    private int seed;
    [SerializeField] private int dungeonStartDepth;
    [SerializeField] private int generationLookahead;
    private string eventValue;

    public Action<string> OnEventChanged { get; set; }
    public GameObject canvas;

    public int DungeonStartDepth
    {
        get => dungeonStartDepth;
        set
        {
            dungeonStartDepth = value;
            CurrentDepth = value;
        }
    }

    public int GenerationLookahead
    {
        get => generationLookahead;
        set
        {
            generationLookahead = value;
        }
    }

    public int Wave { get; set; }
    public int CurrentDepth { get; set; }
    public int LightFailPercentage { get; set; }
    public bool PowerOutage { get; set; } = false;
    public bool PowerOnMode { get; set; } = false;
    public int EnemyAmount { get; set; }
    public bool PlayerInDungeon { get; set; }

    public DifficultyConfig DifficultyConfig
    {
        get
        {
            return UnitySingleton<DifficultyManager>.Instance.Difficulties[(Difficulty) PlayerPrefs.GetInt("Difficulty")];
        }
    }

    public string Event
    {
        get => eventValue;
        set
        {
            eventValue = value;
            OnEventChanged?.Invoke(value);
        }
    }

    public void SetSeed(int seed)
    {
        this.seed = seed;
    }

    public int GetSeed()
    {
        return seed;
    }
}
