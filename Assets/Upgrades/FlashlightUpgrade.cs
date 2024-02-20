using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flashlight Upgrade", menuName = "Upgrades/Flashligt Upgrade")]
public class FlashlightUpgrade : Upgrade
{
    [SerializeField] int intensity;
    [SerializeField] int range;

    protected override object[] Args => new object[] { intensity, range };

    public override void Apply(GameObject playerObject)
    {
        var flashlight = playerObject.transform.parent.GetComponentInChildren<FlashlightController>();
        flashlight.UpgradeLigthing(intensity, range);
    }
}
