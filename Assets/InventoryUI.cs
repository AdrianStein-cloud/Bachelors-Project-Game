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
    }

    public void SetIcon(int index, Sprite icon)
    {
        var image = inventorySlotBorders[index].transform.GetChild(0).GetComponent<Image>();
        image.gameObject.SetActive(true);
        image.sprite = icon;
    }
}
