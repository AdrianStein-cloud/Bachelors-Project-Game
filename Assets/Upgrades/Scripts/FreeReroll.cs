using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Free Reroll", menuName = "Upgrades/Free Reroll")]
public class FreeReroll : Upgrade
{
    public int freeRerollAmount = 1;

    protected override object[] Args => new object[] { ((1 + Stats.Instance.money.IncreaseOnAllMoneyUpgrades) * freeRerollAmount).ToString("0.#") };

    public override void Apply(GameObject player)
    {
        Stats.Instance.money.FreeRerolls += freeRerollAmount;
    }
}
