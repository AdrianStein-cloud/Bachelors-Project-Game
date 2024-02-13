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
                cards = transform.Cast<Transform>().Select(x => x.GetComponent<UpgradeCard>()).ToList();
            }
            return cards;
        } 
    }

    public void SetOnUpgradeCallback(Action<Upgrade> onChooseUpgrade)
    {
        foreach (var card in Cards)
        {
            card.OnChooseUpgrade = (upgrade) =>
            {
                DisableCards();
                onChooseUpgrade(upgrade);
            };
        }
    }

    public void EnableCards(IEnumerable<Upgrade> upgrades)
    {
        int i = 0;
        foreach (var upgrade in upgrades)
        {
            var card = cards[i];
            card.SetUpgrade(upgrade);
            card.gameObject.SetActive(true);
            i++;
        }
    }

    public void DisableCards()
    {
        cards.ForEach(c => c.gameObject.SetActive(false));
    }
}
