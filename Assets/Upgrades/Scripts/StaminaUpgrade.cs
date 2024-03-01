using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stamina Upgrade", menuName = "Upgrades/Stamina Upgrade")]
public class StaminaUpgrade : Upgrade
{
    [SerializeField] float staminaPercentIncrease;
    [SerializeField] float recoverySpeedPercentIncrease;

    protected override object[] Args => new object[] { staminaPercentIncrease, recoverySpeedPercentIncrease };

    public override void Apply(GameObject player)
    {
        var stamina = player.GetComponent<PlayerStamina>();
        stamina.UpgradeStamina(staminaPercentIncrease, recoverySpeedPercentIncrease);
    }
}
