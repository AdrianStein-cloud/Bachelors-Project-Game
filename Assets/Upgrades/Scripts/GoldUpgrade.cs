using UnityEngine;

[CreateAssetMenu(fileName = "Gold Upgrade", menuName = "Upgrades/Gold Upgrade")]
public class GoldUpgrade : Upgrade
{
    public int moreGoldPerHeart = 1;

    protected override object[] Args => new object[] { (int)(moreGoldPerHeart * (1 + Stats.Instance.money.IncreaseOnAllMoneyUpgrades)) };

    public override void Apply(GameObject player)
    {
        UnitySingleton<CurrencyManager>.Instance.currencyPerObjective += (int)(moreGoldPerHeart * (1 + Stats.Instance.money.IncreaseOnAllMoneyUpgrades));
    }
}
