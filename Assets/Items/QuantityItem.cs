
public abstract class QuantityItem : Item
{
    public int quantity;

    protected bool QuantityLeft => currentAmount > 0;
    protected int currentAmount = 0;

    protected virtual void Start()
    {
        currentAmount = quantity;
        UpdateIconText();
        UnitySingleton<GameManager>.Instance.OnWaveOver += () =>
        {
            currentAmount = quantity;
            UpdateIconText();
        };
    }

    protected void SpendQuantity()
    {
        currentAmount--;
        UpdateIconText();
    }

    void UpdateIconText()
    {
        UnitySingleton<Inventory>.Instance.UpdateItemText(this, currentAmount.ToString());
    }
}
