using UnityEngine;

[CreateAssetMenu(fileName = "More Grenades Upgrade", menuName = "Upgrades/More Grenades Upgrade")]
public class ExtraGrenades : Upgrade
{
    public float extraGrenadesPerGrenade = 1;
    public float healthPrecentageDecrease = 20f;

    protected override object[] Args => new object[] { extraGrenadesPerGrenade, (int)(Stats.Instance.player.health * healthPrecentageDecrease / 100) };

    public override void Apply(GameObject player)
    {
        Stats.Instance.grenade.extraGrenadesPerUniqueGrenade += extraGrenadesPerGrenade;
        Stats.Instance.grenade.RefreshGrenades();
        player.GetComponent<PlayerHealth>().AddMaxHealth(-(int)(Stats.Instance.player.health * healthPrecentageDecrease / 100));
    }
}
