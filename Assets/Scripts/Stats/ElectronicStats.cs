using System;
using UnityEngine;

[Serializable]
public class ElectronicStats
{
    public float batteryLifeMultiplier;
    public Action<float> rechargeBattery;
}
