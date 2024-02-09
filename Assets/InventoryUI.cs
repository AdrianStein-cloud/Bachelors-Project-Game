using System.Collections.Generic;
using UnityEngine;

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
}
