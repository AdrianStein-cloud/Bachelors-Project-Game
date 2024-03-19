using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public int currencyPerObjective = 10;

    [SerializeField] int currentCurrency = 0;

    public int Currency
    {
        get
        {
            return currentCurrency;
        }
        private set
        {
            currentCurrency = value;
            currencyText.text = $"{currentCurrency} $";
        }
    } 

    TextMeshProUGUI currencyText;

    private void Awake()
    {
        currencyText = GameObject.Find("Currency").GetComponent<TextMeshProUGUI>();
    }


    public void OnObjectiveCollected()
    {
        Currency += currencyPerObjective;
    }

    public bool Spend(int amount)
    {
        if (Currency >= amount)
        {
            Currency -= amount;
            return true;
        }
        else return false;
    }
}