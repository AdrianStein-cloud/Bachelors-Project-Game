using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speed Upgrade", menuName = "Upgrades/Speed Upgrade")]
public class SpeedUpgrade : Upgrade
{
    [SerializeField] float speedPercentIncrease;

    protected override object[] Args => new object[] { speedPercentIncrease };

    public override void Apply(GameObject playerObject)
    {
        Stats.Instance.player.speedMultiplier += speedPercentIncrease / 100;
    }
}
