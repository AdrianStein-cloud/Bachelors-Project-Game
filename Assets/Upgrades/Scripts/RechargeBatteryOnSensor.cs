using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sensory Recharger", menuName = "Upgrades/Sensory Recharger")]
public class RechargeBatteryOnSensor : Upgrade
{
    public float rechargePercent;

    protected override object[] Args => new object[] { rechargePercent };

    public override void Apply(GameObject player)
    {
        player.transform.parent.GetComponentInChildren<SensorController>().OnSensorBeep += OnSensorBeep;
    }

    void OnSensorBeep(GameObject hit)
    {
        if (hit.CompareTag("Enemy"))
        {
            Stats.Instance.eletronics.rechargeBattery(rechargePercent / 100f);
        }
    }
}
