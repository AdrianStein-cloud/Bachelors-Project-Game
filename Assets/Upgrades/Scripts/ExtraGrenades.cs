using UnityEngine;

[CreateAssetMenu(fileName = "More Grenades Upgrade", menuName = "Upgrades/More Grenades Upgrade")]
public class ExtraGrenades : Upgrade
{
    public float extraGrenadesPerGrenade = 1;
    public float movementSpeedPrecentageDecrease = 20f;

    protected override object[] Args => new object[] { extraGrenadesPerGrenade, movementSpeedPrecentageDecrease };

    public override void Apply(GameObject player)
    {
        Stats.Instance.grenade.extraGrenadesPerUniqueGrenade += extraGrenadesPerGrenade;
        Stats.Instance.grenade.RefreshGrenades();
        Stats.Instance.player.speedMultiplier -= movementSpeedPrecentageDecrease / 100;
    }
}
