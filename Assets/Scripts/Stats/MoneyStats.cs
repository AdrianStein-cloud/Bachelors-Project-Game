using System;

[Serializable]
public class MoneyStats
{
    public float IncreaseOnAllMoneyUpgrades = 0f;

    public int FreeRerolls;

    public int CalculatedFreeRerolls => (int)((1 + IncreaseOnAllMoneyUpgrades) * FreeRerolls);
}
