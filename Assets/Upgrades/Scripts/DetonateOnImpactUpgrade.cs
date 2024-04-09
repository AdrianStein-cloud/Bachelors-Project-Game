using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Detonate on Impact", menuName = "Upgrades/Detonate on Impact")]
public class DetonateOnImpactUpgrade : Upgrade
{
    public override void Apply(GameObject player)
    {
        Stats.Instance.grenade.detonateOnImpact = true;
    }
}
