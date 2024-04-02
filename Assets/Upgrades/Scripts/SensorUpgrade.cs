using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sensor Upgrade", menuName = "Upgrades/Sensor Upgrade")]
public class SensorUpgrade : Upgrade
{
    [SerializeField] int amount;

    protected override object[] Args => new object[] { amount };

    public override void Apply(GameObject player)
    {
        var sensor = player.transform.parent.GetComponentInChildren<SensorController>();
        sensor.Upgrade(amount);
    }
}
