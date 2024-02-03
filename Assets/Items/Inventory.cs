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
        /*InputManager.Actions.Player.PreviousItem.performed += (_) => PreviousItem();
        InputManager.Actions.Player.NextItem.performed += (_) => NextItem();*/
        /*InputManager.Actions.Player.SwitchItem.performed += (ctx) =>
        {
            float scroll = ctx.ReadValue<float>();
            if (scroll > 0) NextItem();
            else if (scroll < 0) PreviousItem();
        };

        InputManager.Actions.Player.ItemPrimary.performed += (ctx) => items[itemIndex]?.Primary();
        InputManager.Actions.Player.ItemSecondary.performed += (ctx) => items[itemIndex]?.Secondary();*/
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

    /*public void AddPrefabItem(GameObject prefabItem)
    {
        if (prefabItem.GetComponent<Item>() != null)
        {
            var go = Instantiate(prefabItem, transform);
            var item = go.GetComponent<Item>();
            Add(item);
        }
        else 
        { 
            Debug.LogWarning($"Item prefab ({prefabItem.name}) doesn't contain the item script"); 
        }
    }*/

    public void Add(Item item)
    {
        if (items.Count >= size)
        {
            Debug.LogWarning("No space in inventory, ignoring item.");
            return;
        }
        items.Add(item);
    }

    void PreviousItem()
    {
        //Debug.Log("Previous item");
        itemIndex = (itemIndex + size - 1) % size;
    }

    void NextItem()
    {
        //Debug.Log("Next item");
        itemIndex = (itemIndex + 1) % size;
    }
}
