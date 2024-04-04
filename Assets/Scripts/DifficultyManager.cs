using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private List<DifficultyConfig> difficultyConfig;
    [SerializeField] private Dictionary<Difficulty, DifficultyConfig> difficulties;

    private void Awake()
    {
        UnitySingleton<DifficultyManager>.BecomeSingleton(this);
        difficulties = new Dictionary<Difficulty, DifficultyConfig>();
        foreach (var difficulty in difficultyConfig)
        {
            difficulties.Add(difficulty.difficulty, difficulty);
        }
    }

    public Dictionary<Difficulty, DifficultyConfig> Difficulties { get { return difficulties; } }
}


[System.Serializable]
public class DifficultyConfig
{
    public Difficulty difficulty;
    public float enemySpawnRate;
    public float lightFailSlope;
    public float lightSlopeMiddleRound;
    public float priceMultiplier;
    public int startMoney;
    public int startMonsters;
}

public enum Difficulty
{
    Normal,
    Hard,
    Nightmare,
    Hell
}
