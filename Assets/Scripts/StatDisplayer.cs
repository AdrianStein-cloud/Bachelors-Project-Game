using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatDisplayer : MonoBehaviour
{
    private TextMeshProUGUI statText;
    [SerializeField] private Upgrade riskyMoves;

    // Start is called before the first frame update
    void Start()
    {
        statText = GetComponentInChildren<TextMeshProUGUI>();
        RefreshStats();
    }

    public void RefreshStats()
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

    public int GetRiskyMoves()
    {
        return ((int)(1 * (1 + Stats.Instance.money.IncreaseOnAllMoneyUpgrades))) * riskyMoves.Purchased;
    }
}
