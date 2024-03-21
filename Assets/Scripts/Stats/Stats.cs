using UnityEngine;

public class Stats : MonoBehaviour
{
    public static Stats Instance;

    public GrenadeStats grenade;
    public ElectronicStats eletronics;
    public PlayerStats player;


    private void Awake()
    {
        Instance = this;
    }
}
