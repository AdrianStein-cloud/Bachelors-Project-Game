using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public abstract class Upgrade : ScriptableObject, IWeighted
{
    public string Name;
    [TextArea]
    [SerializeField] protected string description;
    public Rarity Rarity;
    public Tag Tags;
    public List<Upgrade> NewlyAvailableUpgrades;
    private int price;
    public int Price
    {
        get
        {
            return price + (Purchased * 2);
        }
        set
        {
            price = value;
        }
    }
    public string Description => string.Format(description, Args);

    [field: SerializeField] public int Limit { get; set; }
    public int Purchased { get; set; }

    protected virtual object[] Args => new object[0];

    public int Weight => Rarity.GetChance();

    public abstract void Apply(GameObject player);
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Legendary,
}

[Flags]
public enum Tag
{
    Cooldown = 1 << 0,
    Grenade = 1 << 1,
    Battery = 1 << 2,
    Tablet = 1 << 3,
    Placeable = 1 << 4,
    Quantity = 1 << 5,
    Passive = 1 << 6,
    Money = 1 << 7,
    Teleport = 1 << 8,
}


