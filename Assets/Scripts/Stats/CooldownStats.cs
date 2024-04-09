using System;

[Serializable]
public class CooldownStats
{
    public int RecoverySpeedPercentage;

    public float CalculatedRecoverySpeedMultiplier => 100f / (100 + RecoverySpeedPercentage);
}
