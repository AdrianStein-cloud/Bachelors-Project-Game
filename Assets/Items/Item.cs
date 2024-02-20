using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public string itemName;
    public Sprite icon;

    public virtual void Select() { }
    public virtual void Deselect() { }

    public virtual void Primary() { }
    public virtual void Secondary() { }
}
