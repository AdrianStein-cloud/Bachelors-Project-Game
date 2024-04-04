using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Battery Upgrade", menuName = "Upgrades/Battery Upgrade")]
public class BatteryUpgrade : Upgrade
{
    [SerializeField] int batteryLifePercentage;

    protected override object[] Args => new object[] { batteryLifePercentage };

    public override void Apply(GameObject player)
    {
        Stats.Instance.eletronics.batteryLifeMultiplier += batteryLifePercentage/100f;

        foreach(var batteryItem in GameObject.FindObjectsOfType<BatteryItem>())
        {
            batteryItem.FillBattery();
        }
    }
}
