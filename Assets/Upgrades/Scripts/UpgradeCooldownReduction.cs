using UnityEngine;

[CreateAssetMenu(fileName = "Cooldown Upgrade", menuName = "Upgrades/Cooldown Upgrade")]
public class UpgradeCooldownReuction : Upgrade
{
    [SerializeField] int cooldownRecoverySpeedPrecentage;

    protected override object[] Args => new object[] { cooldownRecoverySpeedPrecentage };

    public override void Apply(GameObject playerObject)
    {
        Stats.Instance.cooldown.RecoverySpeedPercentage += cooldownRecoverySpeedPrecentage;
    }
}
