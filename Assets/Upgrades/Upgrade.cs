using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
    public string Name;
    [SerializeField] string description;
    public string Description => string.Format(description, Args);

    protected virtual object[] Args => new object[0];

    public abstract void Apply(GameObject player);
}
