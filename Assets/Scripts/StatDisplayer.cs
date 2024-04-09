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
            statText.text = "Money: " + UnitySingleton<CurrencyManager>.Instance.Currency + "$\n" +
                        "Health: " + Stats.Instance.player.health + "\n" +
                        "Stamina: " + Stats.Instance.player.stamina + "\n" +
                        "Stamina Recovery: " + Stats.Instance.player.staminaRecovery + "\n" +
                        "Free Rerolls / Wave: " + Stats.Instance.money.FreeRerolls + "\n" +
                        "Bill Gates Increase: " + Stats.Instance.money.IncreaseOnAllMoneyUpgrades * 100 + "%\n" +
                        "Heart Worth: " + Stats.Instance.money.HeartWorth + "$\n" +
                        "Money / Dmg Taken: " + GetRiskyMoves() + "$\n";
        }
        catch (Exception e)
        {
            statText.text = e.Message;
        }
        try
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach(Upgrade upgrade in UnitySingleton<UpgradeManager>.Instance.GetUpgrades().Where(x => x.Purchased > 0))
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
}
