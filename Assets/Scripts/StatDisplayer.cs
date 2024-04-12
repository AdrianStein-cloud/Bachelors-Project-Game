using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public class StatDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statText;
    [SerializeField] private TextMeshProUGUI cardText;

    // Start is called before the first frame update
    void Start()
    {
        RefreshStats();
    }

    public void RefreshStats()
    {
        try
        {
            statText.text = "Wave: " + GameSettings.Instance.Wave + "\n" +
                        "Money: " + UnitySingleton<CurrencyManager>.Instance.Currency + "$\n" +
                        "Health: " + Stats.Instance.player.health + "\n" +
                        "Move Speed: " + Stats.Instance.player.speedMultiplier * 100 + "%\n" +
                        "Stamina: " + Stats.Instance.player.FinalStamina + "%\n" +
                        "Stamina Recovery: " + Stats.Instance.player.FinalStaminaRecovery * 5 + "%\n" +
                        "Free Rerolls / Wave: " + Stats.Instance.money.FreeRerolls + "\n" +
                        "Bill Gates Increase: " + Stats.Instance.money.IncreaseOnAllMoneyUpgrades * 100 + "%\n" +
                        "Heart Worth: " + Stats.Instance.money.HeartWorth + "$\n" +
                        "Money / Dmg Taken: " + GetRiskyMoves() + "$\n" +
                        "Piggy Bank: " + (GetPiggyBank().dungeonEnterGoldPercent / 100f) * Stats.Instance.money.PiggyBanks * (1 + Stats.Instance.money.IncreaseOnAllMoneyUpgrades) * 100 + "%\n" +
                        "Cooldown Recovery Speed: " + Stats.Instance.cooldown.RecoverySpeedPercentage + "%\n" +
                        "Battery Increase: " + Stats.Instance.eletronics.batteryLifeMultiplier * 100 + "%\n";
        }
        catch (Exception e)
        {
            statText.text = e.Message;
        }
        try
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach(Upgrade upgrade in UnitySingleton<UpgradeManager>.Instance.GetUpgrades().Where(x => x.Purchased > 0).OrderBy(x => x.Name))
            {
                stringBuilder.Append(upgrade.Name + ": " + upgrade.Purchased + "\n");
            }
            cardText.text = stringBuilder.ToString();
        }
        catch (Exception e)
        {
            cardText.text = e.Message;
        }
    }

    public int GetRiskyMoves()
    {
        return ((int)(1 * (1 + Stats.Instance.money.IncreaseOnAllMoneyUpgrades))) * UnitySingleton<UpgradeManager>.Instance.GetUpgrades().Where(x => x.name == "Risky Moves").First().Purchased;
    }

    public PiggyBank GetPiggyBank()
    {
        return (PiggyBank) UnitySingleton<UpgradeManager>.Instance.GetUpgrades().Where(x => x.name == "Piggy Bank").First();
    }
}
