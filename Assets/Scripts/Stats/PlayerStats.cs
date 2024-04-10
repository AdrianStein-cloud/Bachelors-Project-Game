using System;

[Serializable]
public class PlayerStats
{
    public int keysHeld = 0;
    public float speedMultiplier = 1f;
    public int health = 100;
    public float stamina;
    public float staminaRecovery;
    public float staminaIncrease;
    public float staminaRecoveryIncrease;

    public float FinalStamina => stamina * (1 + (staminaIncrease / 100));
    public float FinalStaminaRecovery => staminaRecovery * (1 + (staminaRecoveryIncrease / 100));

    public void AddKey()
    {
        keysHeld++;
        InventoryUI.Instance.SetKeyImageActive(true);
    }

    public void RemoveKey()
    {
        keysHeld--;
        if(keysHeld <= 0)
        {
            InventoryUI.Instance.SetKeyImageActive(false);
        }
    }
}
