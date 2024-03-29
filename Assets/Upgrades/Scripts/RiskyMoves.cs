using UnityEngine;

[CreateAssetMenu(fileName = "Risky Moves", menuName = "Upgrades/Risky Moves")]
public class RiskyMoves : Upgrade
{
    public override void Apply(GameObject player)
    {
        player.GetComponentInChildren<PlayerHealth>().OnTakeDamage += (damage) =>
        {
            int earned = (int)(damage * (1 + Stats.Instance.money.IncreaseOnAllMoneyUpgrades));
            UnitySingleton<CurrencyManager>.Instance.AddCurrency(earned);
        };
    }
}
