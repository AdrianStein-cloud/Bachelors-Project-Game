using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flashlight Upgrade", menuName = "Upgrades/Flashligt Upgrade")]
public class FlashlightUpgrade : Upgrade
{
    [SerializeField] int intensity;
    [SerializeField] int range;
    [SerializeField] int angle;
    [SerializeField] int batteryLife;

    protected override object[] Args => new object[] { intensity, range, angle, batteryLife };

    public override void Apply(GameObject playerObject)
    {
        var flashlight = playerObject.transform.parent.GetComponentInChildren<FlashlightController>();
        flashlight.Upgrade(intensity, range, angle, batteryLife);
    }
}
