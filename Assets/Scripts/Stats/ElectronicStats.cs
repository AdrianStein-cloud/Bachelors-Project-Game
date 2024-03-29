using System;

[Serializable]
public class ElectronicStats
{
    public float batteryLifeMultiplier;
    public Action<float> rechargeBattery;
}
