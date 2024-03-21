using System;

[Serializable]
public class GrenadeStats
{
    public bool detonateOnImpact = false;
    public int grenadeAmountMultiplier = 1;
    public Action onGrenadeExplosion;
}
