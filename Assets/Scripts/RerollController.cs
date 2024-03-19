using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RerollController : Clickable
{
    TextMeshProUGUI text;

    TextMeshProUGUI PriceText
    {
        get
        {
            if (text == null)
            {
                text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
            }
            return text;
        }
    }

    public void SetRerollPrice(int price)
    {
        PriceText.text = $"Reroll {price}$";
    }
}
