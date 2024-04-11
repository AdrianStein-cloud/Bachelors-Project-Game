using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public TextMeshProUGUI primaryDescription;
    public TextMeshProUGUI secondaryDescription;
    public int size;
    public List<Item> items;

    public int itemIndex = 0;

    int lastIndex = 0;
    bool inventoryFull = false;
    GameObject primaryGO;
    GameObject secondaryGO;

    public Action OnInventoryFull { get; set; }

    private void Awake()
    {
        UnitySingleton<Inventory>.BecomeSingleton(this);
        primaryGO = primaryDescription.transform.parent.gameObject;
        secondaryGO = secondaryDescription.transform.parent.gameObject;
    }

    private void Start()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null) InventoryUI.Instance.SetIcon(i, items[i].icon);
        }

        //items[itemIndex]?.Select();
        itemIndex = -1;
        SetItemIndex(0);

        InputManager.Player.SwitchItemNumKeys.performed += SwitchItem;
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

    public Item Add(Item item)
    {
        int index = items.IndexOf(null);
        if (index == -1)
        {
            Debug.LogWarning("No space in inventory, ignoring item.");
            return null;
        }
        var itemObject = Instantiate(item.gameObject, transform);
        item = itemObject.GetComponent<Item>();
        items[index] = item;
        InventoryUI.Instance.SetIcon(index, item.icon);
        items[itemIndex]?.Select();

        if (!inventoryFull && items.Where(x => x != null).Count() >= size)
        {
            inventoryFull = true;
            OnInventoryFull?.Invoke();
        }

        return item;
    }

    public void UpdateItemText(Item item, string text)
    {
        InventoryUI.Instance.SetText(items.IndexOf(item), text);
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

    void SwitchItem(InputAction.CallbackContext ctx)
    {
        var index = (int)ctx.ReadValue<float>() - 1;
        SetItemIndex(index == -1 ? lastIndex : index);
        lastIndex = index == -1 ? lastIndex : index;
    }

    void SetItemIndex(int value)
    {
        if (itemIndex == value) return;
        if (itemIndex >= 0 && itemIndex < size) items[itemIndex]?.Deselect();
        itemIndex = value;
        InventoryUI.Instance.SetCurrentSlot(value);
        items[itemIndex]?.Select();

        if (items[itemIndex] == null)
        {
            primaryGO.SetActive(false);
            secondaryGO.SetActive(false);
            return;
        }

        primaryGO.SetActive(true);
        secondaryGO.SetActive(items[itemIndex].secondaryDescription.Length > 0);
        primaryDescription.text = items[itemIndex].primaryDescription;
        secondaryDescription.text = items[itemIndex].secondaryDescription;

        StopAllCoroutines();
        StartCoroutine(HideDescriptions());
    }
    
    IEnumerator HideDescriptions()
    {
        yield return new WaitForSeconds(5f);
        primaryGO.SetActive(false);
        secondaryGO.SetActive(false);
    }
}
