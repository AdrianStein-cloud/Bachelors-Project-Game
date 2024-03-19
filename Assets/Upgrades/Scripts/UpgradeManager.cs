using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using System;

public class UpgradeManager : MonoBehaviour, IUpgradeManager
{
    public int rerollPrice = 20;

    [SerializeField] List<Upgrade> startUpgrades;
    [SerializeField] List<Upgrade> upgrades;

    UpgradeUIController upgradeUIController;
    GameObject player;
    int selectionAmount;

    Action DoneChoosingUpgrades;

    List<Upgrade> availableUpgrades;

    CurrencyManager currencyManager;

    readonly System.Random random = new System.Random();


    private void Start()
    {
        currencyManager = GetComponent<CurrencyManager>();
        availableUpgrades = new List<Upgrade>(upgrades);
        upgradeUIController = FindAnyObjectByType<UpgradeUIController>();
        upgradeUIController.Init(this);
        upgradeUIController.SetRerollPrice(rerollPrice);

        player = GameObject.FindWithTag("Player");

        foreach (var upgrade in startUpgrades)
        {
            upgrade.Apply(player);
        }
    }

    public void DisplayUpgrades(int amount, GameObject player, Action upgradeChosen)
    {
        this.player = player;
        this.DoneChoosingUpgrades = upgradeChosen;
        selectionAmount = amount;
        DisplayRandomUpgrades();
    }

    void DisplayRandomUpgrades()
    {
        var upgradesCopy = new List<Upgrade>(availableUpgrades);
        var randomUpgrades = Enumerable.Range(0, Math.Min(selectionAmount, upgradesCopy.Count)).Select(_ => {
            var upgrade = upgradesCopy.GetRollFromWeights(random);
            upgradesCopy.Remove(upgrade);
            return upgrade;
        });
        upgradeUIController.EnableCards(randomUpgrades);
    }

    public void ChooseUpgrade(Upgrade upgrade)
    {
        if (currencyManager.Spend(10))
        {
            upgradeUIController.RemoveUpgrade(upgrade);
            availableUpgrades.Remove(upgrade);
            availableUpgrades.AddRange(upgrade.NewlyAvailableUpgrades);
            upgrade.Apply(player);
        }
    }

    public void Reroll()
    {
        if (currencyManager.Spend(rerollPrice))
        {
            upgradeUIController.SetRerollPrice(rerollPrice);
            DisplayRandomUpgrades();
        }
    }

    public void CloseUpgrades()
    {
        DoneChoosingUpgrades();
    }
}

public interface IUpgradeManager
{
    void Reroll();
    void ChooseUpgrade(Upgrade upgrade);
    void CloseUpgrades();
}
