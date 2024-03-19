using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UpgradeUIController : MonoBehaviour
{
    List<UpgradeCard> cards;
    List<UpgradeCard> Cards { 
        get
        {
            if(cards == null)
            {
                cards = GetComponentsInChildren<UpgradeCard>(true).ToList();
            }
            return cards;
        } 
    }


    RerollController rerollController;
    RerollController RerollController
    {
        get
        {
            if (rerollController == null)
            {
                rerollController = GetComponentInChildren<RerollController>(true);
            }
            return rerollController;
        }
    }

    CloseUpgrades closeButton;
    CloseUpgrades CloseButton
    {
        get
        {
            if (closeButton == null)
            {
                closeButton = GetComponentInChildren<CloseUpgrades>(true);
            }
            return closeButton;
        }
    }

    public void Init(IUpgradeManager upgradeManager)
    {

        Cards.ForEach(card => card.OnClick = () => upgradeManager.ChooseUpgrade(card.Upgrade));
        RerollController.OnClick = upgradeManager.Reroll;
        CloseButton.OnClick += CloseUpgrades;
        CloseButton.OnClick += upgradeManager.CloseUpgrades;
    }

    public void EnableCards(IEnumerable<Upgrade> upgrades)
    {
        Cards.ForEach(c => c.gameObject.SetActive(false));
        RerollController.gameObject.SetActive(true);
        CloseButton.gameObject.SetActive(true);
        int i = 0;
        foreach (var upgrade in upgrades)
        {
            var card = cards[i];
            card.SetUpgrade(upgrade);
            card.gameObject.SetActive(true);
            i++;
        }
    }

    public void SetRerollPrice(int price)
    {
        RerollController.SetRerollPrice(price);
    }

    public void RemoveUpgrade(Upgrade upgrade)
    {
        Cards.Where(c => c.Upgrade == upgrade).First().gameObject.SetActive(false);
    }

    public void CloseUpgrades()
    {
        Cards.ForEach(c => c.gameObject.SetActive(false));
        RerollController.gameObject.SetActive(false);
        CloseButton.gameObject.SetActive(false);
    }
}
