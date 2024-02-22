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
        GameSettings.Instance.CurrentDepth = GameSettings.Instance.DungeonStartDepth + GameSettings.Instance.Wave / 2;
        GameSettings.Instance.LightFailPercentage = (int) (85f / (1f + Mathf.Exp(-0.25f * (GameSettings.Instance.Wave - 10f))) + 10f);
        GameSettings.Instance.EnemyAmount = (int) MathF.Floor(1 + GameSettings.Instance.Wave / 5);
    }

    public void RestartDanger()
    {
        GameSettings.Instance.Wave = 0;
        GameSettings.Instance.CurrentDepth = GameSettings.Instance.DungeonStartDepth;
        GameSettings.Instance.EnemyAmount = 1;
    }
}
