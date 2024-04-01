using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public List<GameObject> inventorySlotBorders;

    public GameObject currentSlot;

    private void Awake()
    {
        Instance = this;
    }

    public void SetCurrentSlot(int index)
    {
        currentSlot.transform.position = inventorySlotBorders[index].transform.position;
        currentSlot.GetComponent<Image>().sprite = inventorySlotBorders[index].GetComponent<Image>().sprite;
    }

    public void SetIcon(int index, Sprite icon)
    {
        var slotUI = inventorySlotBorders[index].GetComponent<InventorySlotUI>();
        slotUI.Icon.gameObject.SetActive(true);
        slotUI.Icon.sprite = icon;
    }

    public void SetText(int index, string text)
    {
        var slotUI = inventorySlotBorders[index].GetComponent<InventorySlotUI>();
        slotUI.Text.gameObject.SetActive(true);
        slotUI.Text.text = text;
    }
}
