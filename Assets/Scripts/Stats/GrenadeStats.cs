using System;
using System.Collections.Generic;

[Serializable]
public class GrenadeStats
{
    public bool detonateOnImpact = false;
    public float extraGrenadesPerUniqueGrenade = 0;
    public Action onGrenadeExplosion;
    public int ExtraGrenades => (int)(Grenades.Count * extraGrenadesPerUniqueGrenade);
    public List<ThrowableItem> Grenades { get; set; } = new List<ThrowableItem>();

    public void RefreshGrenades()
    {
        Grenades.ForEach(g => g.RefreshGrenade());
    }
}
