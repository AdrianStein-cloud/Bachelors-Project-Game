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
    }

    public void RestartDanger()
    {
        GameSettings.Instance.Wave = 0;
        GameSettings.Instance.CurrentDepth = GameSettings.Instance.DungeonStartDepth;
    }
}
