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
    public string Description => string.Format(description, Args);

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
    Throwable = 1 << 1,
    Electronic = 1 << 2,
    Tablet = 1 << 3,
    Placeable = 1 << 4,
    Quantity = 1 << 5,
    Passive = 1 << 6,
}


