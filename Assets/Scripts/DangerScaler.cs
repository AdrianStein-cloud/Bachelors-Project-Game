using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerScaler
{
    public DangerScaler()
    {
        RestartDanger();
    }

    public void ScaleDanger()
    {
        GameSettings.Instance.Wave++;
        GameSettings.Instance.CurrentDepth = (int) Mathf.Floor(GameSettings.Instance.DungeonStartDepth + GameSettings.Instance.Wave / 2.5f);
        GameSettings.Instance.LightFailPercentage = (int) (77.2f / (1f + Mathf.Exp(-GameSettings.Instance.DifficultyConfig.lightFailSlope * (GameSettings.Instance.Wave - GameSettings.Instance.DifficultyConfig.lightSlopeMiddleRound))) + 12.8f);
        if(GameSettings.Instance.Wave == 1)
        {
            GameSettings.Instance.EnemyAmount = GameSettings.Instance.DifficultyConfig.startMonsters;
        }
        else
        {
            GameSettings.Instance.EnemyAmount = (int) MathF.Ceiling((GameSettings.Instance.Wave - 1) / GameSettings.Instance.DifficultyConfig.enemySpawnRate);
        }
    }

    public void RestartDanger()
    {
        GameSettings.Instance.Wave = 0;
        GameSettings.Instance.CurrentDepth = GameSettings.Instance.DungeonStartDepth;
        GameSettings.Instance.EnemyAmount = 1;
    }
}
