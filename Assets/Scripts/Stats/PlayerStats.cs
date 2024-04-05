using System;

[Serializable]
public class PlayerStats
{
    public int keysHeld = 0;
    public float speedMultiplier = 1f;

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
