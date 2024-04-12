using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Piggy Bank", menuName = "Upgrades/Piggy Bank")]
public class PiggyBank : Upgrade
{
    public int dungeonEnterGoldPercent = 20;

    protected override object[] Args => new object[] { (int)(dungeonEnterGoldPercent * (1 + Stats.Instance.money.IncreaseOnAllMoneyUpgrades)) };

    public override void Apply(GameObject player)
    {
        if (Stats.Instance.money.PiggyBanks == 0)
        {
            UnitySingleton<GameManager>.Instance.OnDungeonEnter += () =>
            {
                var currencyManager = UnitySingleton<CurrencyManager>.Instance;
                float returnOnSavings = currencyManager.Currency * (dungeonEnterGoldPercent / 100f) * Stats.Instance.money.PiggyBanks * (1 + Stats.Instance.money.IncreaseOnAllMoneyUpgrades);
                currencyManager.AddCurrency((int)returnOnSavings);
            };
        }
        Stats.Instance.money.PiggyBanks++;
    }
}
