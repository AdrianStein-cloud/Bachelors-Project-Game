using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public int size;
    public List<Item> items;

    public int itemIndex = 0;

    private void Start()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null) InventoryUI.Instance.SetIcon(i, items[i].icon);
        }

        items[itemIndex]?.Select();
    }

    private void Update()
    {
        if (InputManager.Player.SwitchItem.triggered)
        {
            float scroll = InputManager.Player.SwitchItem.ReadValue<float>();
            if (scroll > 0) NextItem();
            else if (scroll < 0) PreviousItem();
        }
        else if(InputManager.Player.ItemPrimary.triggered) items[itemIndex]?.Primary();
        else if(InputManager.Player.ItemSecondary.triggered) items[itemIndex]?.Secondary();
    }

    public void Add(Item item)
    {
        int index = items.IndexOf(null);
        if (index == -1)
        {
            Debug.LogWarning("No space in inventory, ignoring item.");
            return;
        }
        var itemObject = Instantiate(item.gameObject, transform);
        item = itemObject.GetComponent<Item>();
        items[index] = item;
        InventoryUI.Instance.SetIcon(index, item.icon);
    }

    void PreviousItem()
    {
        //Debug.Log("Previous item");
        SetItemIndex((itemIndex + size - 1) % size);

    }

    void NextItem()
    {
        //Debug.Log("Next item");
        SetItemIndex((itemIndex + 1) % size);
    }

    void SetItemIndex(int value)
    {
        items[itemIndex]?.Deselect();
        itemIndex = value;
        InventoryUI.Instance.SetCurrentSlot(value);
        items[itemIndex]?.Select();
    }
}
