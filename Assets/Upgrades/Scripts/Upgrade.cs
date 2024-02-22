using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
    public string Name;
    [TextArea]
    [SerializeField] protected string description;
    public List<Upgrade> NewlyAvailableUpgrades;
    public string Description => string.Format(description, Args);

    protected virtual object[] Args => new object[0];

    public abstract void Apply(GameObject player);
}