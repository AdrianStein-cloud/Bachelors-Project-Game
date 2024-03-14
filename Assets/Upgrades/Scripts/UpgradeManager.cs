using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using System;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] List<Upgrade> startUpgrades;
    [SerializeField] List<Upgrade> upgrades;

    UpgradeUIController upgradeUIController;
    GameObject player;

    Action upgradeChosen;

    List<Upgrade> availableUpgrades;

    private void Start()
    {
        availableUpgrades = new List<Upgrade>(upgrades);
        upgradeUIController = FindAnyObjectByType<UpgradeUIController>();
        upgradeUIController.SetOnUpgradeCallback(ChooseUpgrade);

        player = GameObject.FindWithTag("Player");

        foreach (var upgrade in startUpgrades)
        {
            upgrade.Apply(player);
        }
    }

    public void DisplayUpgrades(int amount, GameObject player, Action upgradeChosen)
    {
        this.player = player;
        this.upgradeChosen = upgradeChosen;
        var upgradesCopy = new List<Upgrade>(availableUpgrades);
        var randomUpgrades = Enumerable.Range(0, Math.Min(amount, upgradesCopy.Count)).Select(_ => {
            var upgrade = upgradesCopy[UnityEngine.Random.Range(0, upgradesCopy.Count)];
            upgradesCopy.Remove(upgrade);
            return upgrade;
        });
        upgradeUIController.EnableCards(randomUpgrades);
    }

    void ChooseUpgrade(Upgrade upgrade)
    {
        availableUpgrades.Remove(upgrade);
        availableUpgrades.AddRange(upgrade.NewlyAvailableUpgrades);
        upgrade.Apply(player);
        upgradeChosen();
    }
}
