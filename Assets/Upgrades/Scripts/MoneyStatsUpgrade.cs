using UnityEngine;

[CreateAssetMenu(fileName = "Money Stats", menuName = "Upgrades/Money Stats")]
public class MoneySatsUpgrade : Upgrade
{
    public float increaseOnAllMoneyUpgradesPercent = 20f;

    protected override object[] Args => new object[] { (int)(increaseOnAllMoneyUpgradesPercent * (1 + Stats.Instance.money.IncreaseOnAllMoneyUpgrades)) };

    public override void Apply(GameObject player)
    {
        Stats.Instance.money.IncreaseOnAllMoneyUpgrades += increaseOnAllMoneyUpgradesPercent / 100f * (1 + Stats.Instance.money.IncreaseOnAllMoneyUpgrades);
    }
}
