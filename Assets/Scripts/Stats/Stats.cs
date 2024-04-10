using UnityEngine;

public class Stats : MonoBehaviour
{
    public static Stats Instance;

    public GrenadeStats grenade;
    public ElectronicStats eletronics;
    public PlayerStats player;
    public MoneyStats money;
    public CooldownStats cooldown;
    public TeleportStats teleport;


    private void Awake()
    {
        Instance = this;
    }
}
