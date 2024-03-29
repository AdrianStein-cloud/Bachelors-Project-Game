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
    [SerializeField] int selectionAmount;

    Action DoneChoosingUpgrades;

    List<Upgrade> availableUpgrades;

    List<Upgrade> currentUpgrades;

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

        RefreshUpgrades();

        foreach (var upgrade in startUpgrades)
        {
            upgrade.Apply(player);
        }
    }

    public void DisplayUpgrades(Action upgradeChosen = null)
    {
        InputManager.Player.Disable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        this.DoneChoosingUpgrades = upgradeChosen;
        upgradeUIController.EnableCards(currentUpgrades);
    }

    public void RefreshUpgrades()
    {
        var upgradesCopy = new List<Upgrade>(availableUpgrades);
        var randomUpgrades = Enumerable.Range(0, Math.Min(selectionAmount, upgradesCopy.Count)).Select(_ => {
            var upgrade = upgradesCopy.GetRollFromWeights(random);
            upgradesCopy.Remove(upgrade);
            return upgrade;
        });

        currentUpgrades = randomUpgrades.ToList();

        //Randomize prices slightly
        currentUpgrades.ForEach(u =>
        {
            int normalPrice = u.Rarity.GetPrice();
            int maxDiscount = normalPrice / 5;
            int discount = UnityEngine.Random.Range(0, maxDiscount);
            u.Price = normalPrice - discount;
        });
    }

    public void ChooseUpgrade(Upgrade upgrade)
    {
        if (currencyManager.Spend(upgrade.Price))
        {
            upgradeUIController.RemoveUpgrade(upgrade);
            availableUpgrades.Remove(upgrade);
            currentUpgrades.Remove(upgrade);
            availableUpgrades.AddRange(upgrade.NewlyAvailableUpgrades);
            upgrade.Apply(player);
        }
    }

    public void Reroll()
    {
        if (currencyManager.Spend(rerollPrice))
        {
            RefreshUpgrades();
            upgradeUIController.EnableCards(currentUpgrades);
            upgradeUIController.SetRerollPrice(rerollPrice);
        }
    }

    public void CloseUpgrades()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InputManager.Player.Enable();
        DoneChoosingUpgrades?.Invoke();
    }
}

public interface IUpgradeManager
{
    void Reroll();
    void ChooseUpgrade(Upgrade upgrade);
    void CloseUpgrades();
}
