using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private int currencyPerObjective = 10;

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

    public void IncreaseObjectiveWorth(int increase)
    {
        currencyPerObjective += (int)(increase * (1 + Stats.Instance.money.IncreaseOnAllMoneyUpgrades));
        Stats.Instance.money.HeartWorth = currencyPerObjective;
    }

    TextMeshProUGUI currencyText;

    private void Awake()
    {
        UnitySingleton<CurrencyManager>.BecomeSingleton(this);
        currencyText = GameObject.Find("Currency").GetComponent<TextMeshProUGUI>();
        currencyText.text = $"{currentCurrency} $";
    }

    private void Start()
    {
        AddCurrency(GameSettings.Instance.DifficultyConfig.startMoney);
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

    public void AddCurrency(int amount)
    {
        Currency += amount;
    }
}
