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
        GameSettings.Instance.LightFailPercentage = (int) (77.2f / (1f + Mathf.Exp(-1f * (GameSettings.Instance.Wave - 4.3f))) + 12.8f);
        GameSettings.Instance.EnemyAmount = (int) MathF.Ceiling((GameSettings.Instance.Wave - 1) / 3f);
    }

    public void RestartDanger()
    {
        GameSettings.Instance.Wave = 0;
        GameSettings.Instance.CurrentDepth = GameSettings.Instance.DungeonStartDepth;
        GameSettings.Instance.EnemyAmount = 1;
    }
}
