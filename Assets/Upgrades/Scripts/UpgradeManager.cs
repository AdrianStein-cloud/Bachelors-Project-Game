using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using System;

public class UpgradeManager : MonoBehaviour
{
    public List<Upgrade> Upgrades;

    UpgradeUIController upgradeUIController;
    GameObject player;

    Action upgradeChosen;

    private void Start()
    {
        upgradeUIController = FindAnyObjectByType<UpgradeUIController>();
        upgradeUIController.SetOnUpgradeCallback(ChooseUpgrade);
    }

    public void DisplayUpgrades(int amount, GameObject player, Action upgradeChosen)
    {
        this.player = player;
        this.upgradeChosen = upgradeChosen;
        var upgradesCopy = new List<Upgrade>(Upgrades);
        var randomUpgrades = Enumerable.Range(0, Math.Min(amount, upgradesCopy.Count)).Select(_ => {
            var upgrade = upgradesCopy[UnityEngine.Random.Range(0, upgradesCopy.Count)];
            upgradesCopy.Remove(upgrade);
            return upgrade;
        });
        upgradeUIController.EnableCards(randomUpgrades);
    }

    void ChooseUpgrade(Upgrade upgrade)
    {
        Upgrades.Remove(upgrade);
        Upgrades.AddRange(upgrade.NewlyAvailableUpgrades);
        upgrade.Apply(player);
        upgradeChosen();
    }
}
