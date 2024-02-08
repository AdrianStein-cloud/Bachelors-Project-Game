using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Upgrade", menuName = "Upgrades/Item Upgrade")]
public class ItemUpgrade : Upgrade
{
    [SerializeField] Item item;

    protected override object[] Args => new object[] { item.itemName };

    public override void Apply(GameObject player)
    {
        var inventory = player.GetComponentInChildren<Inventory>();
        inventory.Add(item);
    }
}
