using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : ScriptableObject, IWeighted
{
    public string Name;
    [TextArea]
    [SerializeField] protected string description;
    public Rarity Rarity;
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
    Epic,
    Legendary,
}
